using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using FitAndGym.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace FitAndGym.ViewModels
{
    public class FitAndGymViewModel : INotifyPropertyChanged
    {
        // Constructor & DataContext initialization
        private FitAndGymDataContext db;

        public FitAndGymViewModel() { } // to fulfill Design-Time data access requirements
        public FitAndGymViewModel(string fitAndGymConnectionString)
        {
            db = new FitAndGymDataContext(fitAndGymConnectionString);
            db.Log = new Utilities.DebugTextWriter();
        }

        #region Application's DB Collections

        private ObservableCollection<TrainingDay> _trainingDays;
        public ObservableCollection<TrainingDay> TrainingDays
        {
            get { return _trainingDays; }
            set
            {
                _trainingDays = value;
                NotifyPropertyChanged("TrainingDays");
            }
        }

        private ObservableCollection<Exercise> _exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get { return _exercises; }
            set
            {
                _exercises = value;
                NotifyPropertyChanged("Exercises");
            }
        }

        public List<TrainingDay> IncomingTrainingDays
        {
            get
            {
                return _trainingDays.Where(x => x.StartTime >= (x.DurationInMinutes.HasValue ? (DateTime.Now - TimeSpan.FromSeconds(x.DurationInMinutes.Value)) : DateTime.Now)).Take(6).ToList();
            }
        }

        #endregion

        public async Task LoadTrainingDaysCollectionFromDatabase()
        {
            await Task.Factory.StartNew(() =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var trainingDaysInDB = from TrainingDay trDay
                                           in db.TrainingDays
                                           orderby trDay.StartTime
                                           select trDay;

                    TrainingDays = new ObservableCollection<TrainingDay>(trainingDaysInDB);
                });
            });
        }

        public async Task LoadExercisesCollectionFromDatabase()
        {
            await Task.Factory.StartNew(() =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var exercisesInDB = from Exercise ex
                                        in db.Exercises
                                        orderby ex.ExerciseName
                                        select ex;

                    Exercises = new ObservableCollection<Exercise>(exercisesInDB);
                });
            });
        }
        
        public void AddNewExercise(Exercise newExercise)
        {
            int index = 0;

            foreach (Exercise exercise in Exercises.ToList())
            {
                if (newExercise.ExerciseName.CompareTo(exercise.ExerciseName) <= 0) break;

                ++index;
            }

            Exercises.Insert(index, newExercise);

            db.Exercises.InsertOnSubmit(newExercise);
            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges()); 
        }

        public void UpdateExercise(Exercise exerciseToUpdate)
        {
            Exercise exToUpdate = db.Exercises.FirstOrDefault(ex => ex.ExerciseId == exerciseToUpdate.ExerciseId);
            if (exToUpdate == null) throw new Exception("Exercise to edit not found - from UpdateExercise");

            if (exToUpdate.ExerciseName != exerciseToUpdate.ExerciseName)
            {
                Exercises.Remove(exToUpdate);

                int index = 0;

                foreach (Exercise exercise in Exercises.ToList())
                {
                    if (exerciseToUpdate.ExerciseName.CompareTo(exercise.ExerciseName) <= 0) break;

                    ++index;
                }

                Exercises.Insert(index, exToUpdate);
            }

            exToUpdate.AmountOfReps = exerciseToUpdate.AmountOfReps;
            exToUpdate.AmountOfSets = exerciseToUpdate.AmountOfSets;
            exToUpdate.DurationInMinutes = exerciseToUpdate.DurationInMinutes;
            exToUpdate.ExerciseName = exerciseToUpdate.ExerciseName;
            exToUpdate.ImageUri = exerciseToUpdate.ImageUri;
            exToUpdate.Intensity = exerciseToUpdate.Intensity;
            exToUpdate.OtherInfo = exerciseToUpdate.OtherInfo;

            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges()); 
        }

        public Exercise GetExerciseById(int exId)
        {
            return db.Exercises.FirstOrDefault(ex => ex.ExerciseId == exId);
        }

        public TrainingDay GetTrainingById(int trId)
        {
            return db.TrainingDays.FirstOrDefault(tr => tr.TrainingDayId == trId);
        }

        public void DeleteExercise(Exercise exerciseToDelete)
        {
            Exercise exToDelete = GetExerciseById(exerciseToDelete.ExerciseId);
            if (exToDelete == null) throw new Exception("Exercise to delete not found - from DeleteExercise");

            Exercises.Remove(exToDelete);
            db.Exercises.DeleteOnSubmit(exToDelete);

            db.ExTrDayConnectors.DeleteAllOnSubmit(
                db.ExTrDayConnectors.Where(x =>
                    x._exerciseId == exerciseToDelete.ExerciseId));

            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges()); 
        }

        public void DeleteTrainingsByDate(DateTime dateToWhichDelete)
        {
            var trainingsToDelete = db.TrainingDays.Where(x => x.StartTime < dateToWhichDelete);

            foreach (var item in trainingsToDelete)
            {
                TrainingDays.Remove(item);

                db.ExTrDayConnectors.DeleteAllOnSubmit(
                    db.ExTrDayConnectors.Where(x =>
                        x._trainingDayId == item.TrainingDayId));
            }

            db.TrainingDays.DeleteAllOnSubmit(trainingsToDelete);

            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges());
        }

        public void DeleteTraining(TrainingDay trainingToDelete)
        {
            TrainingDay trToDelete = GetTrainingById(trainingToDelete.TrainingDayId);
            if (trToDelete == null) throw new Exception("Training to delete not found - from DeleteTraining");

            TrainingDays.Remove(trToDelete);
            NotifyPropertyChanged("IncomingTrainingDays");

            db.TrainingDays.DeleteOnSubmit(trToDelete);

            db.ExTrDayConnectors.DeleteAllOnSubmit(
                db.ExTrDayConnectors.Where(x =>
                    x._trainingDayId == trainingToDelete.TrainingDayId));

            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges()); 
        }

        private void DeleteOldTrainings(TimeSpan youngestAgeOfTrainingToDelete)
        {
            var toDelete = db.TrainingDays.Where(x =>
                (x.DurationInMinutes.HasValue
                    ? x.StartTime + TimeSpan.FromSeconds(x.DurationInMinutes.Value)
                    : x.StartTime) < (DateTime.Now - youngestAgeOfTrainingToDelete));

            foreach (var item in toDelete)
            {
                TrainingDays.Remove(item);
                IncomingTrainingDays.Remove(item);
                NotifyPropertyChanged("IncomingTrainingDays");

                db.ExTrDayConnectors.DeleteAllOnSubmit(
                    db.ExTrDayConnectors.Where(x =>
                        x._trainingDayId == item.TrainingDayId));
            }

            db.TrainingDays.DeleteAllOnSubmit(toDelete);

            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges()); 
        }

        public void CopyTraining()
        {
            throw new NotImplementedException();
        }

        public void UpdateTraining(TrainingDay trainingToUpdate)
        {
            int index = 0;

            TrainingDay trToUpdate = GetTrainingById(trainingToUpdate.TrainingDayId);
            if (trToUpdate == null) throw new Exception("Training to edit not found - from UpdateTraining");

            TrainingDays.Remove(trToUpdate);
            db.TrainingDays.DeleteOnSubmit(trToUpdate);

            db.ExTrDayConnectors.DeleteAllOnSubmit(
                db.ExTrDayConnectors.Where(x =>
                    x._trainingDayId == trainingToUpdate.TrainingDayId));

            foreach (TrainingDay training in TrainingDays.ToList())
            {
                if (trainingToUpdate.StartTime <= training.StartTime) break;
                ++index;
            }

            TrainingDays.Insert(index, trainingToUpdate);

            trainingToUpdate.TrainingDayId = 0;
            db.TrainingDays.InsertOnSubmit(trainingToUpdate);

            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges()); 
        }

        public void AddNewTraining(TrainingDay newTraining)
        {
            int index = 0;

            foreach (TrainingDay training in TrainingDays.ToList())
            {
                if (newTraining.StartTime <= training.StartTime) break;
                ++index;
            }

            TrainingDays.Insert(index, newTraining);

            db.TrainingDays.InsertOnSubmit(newTraining);
            Deployment.Current.Dispatcher.BeginInvoke(() => db.SubmitChanges()); 
        }

        public Dictionary<DateTime, int> GetNumberOfTrainingsPerDayByMonth(DateTime month)
        {
            var toReturn = new Dictionary<DateTime, int>();
            toReturn = db.TrainingDays
                .GroupBy(x => x.StartTime.Date)
                .ToDictionary(gdc => gdc.Key, gdc => gdc.ToList().Sum(s => s.ExConns.Count()));

            return toReturn;
        }

        public IEnumerable<TrainingDay> GetTrainingsByDate(DateTime date)
        {
            return db.TrainingDays.Where(x => x.StartTime.Date == date);
        }

        #region Events Stuff

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}