using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using FitAndGym.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace FitAndGym.DB
{
    public class FitAndGymDBMethods
    {
        // Constructor & DataContext initialization
        private FitAndGymDataContext db;

        public FitAndGymDBMethods() { } // to fulfill Design-Time data access requirements
        public FitAndGymDBMethods(string fitAndGymConnectionString)
        {
            db = new FitAndGymDataContext(fitAndGymConnectionString);
            db.Log = new Utilities.DebugTextWriter();
        }

        public IEnumerable<TrainingDay> GetTrainingsFromDatabase(int numOfTrainingsToLoad = 0)
        {
            var trainingDaysInDB = from TrainingDay trDay
                                   in db.TrainingDays
                                   orderby trDay.StartTime

                                   select trDay;

            return numOfTrainingsToLoad <= 0
                ? trainingDaysInDB
                : trainingDaysInDB.Take(numOfTrainingsToLoad);
        }

        public IEnumerable<Exercise> LoadExercisesCollectionFromDatabase()
        {
            var exercisesInDB = from Exercise ex
                                in db.Exercises
                                orderby ex.ExerciseName
                                select ex;

            return exercisesInDB;
        }
        
        public void AddNewExercise(Exercise newExercise)
        {
            db.Exercises.InsertOnSubmit(newExercise);
            db.SubmitChanges(); 
        }

        public void UpdateExercise(Exercise exerciseToUpdate)
        {
            Exercise exToUpdate = db.Exercises.FirstOrDefault(ex => ex.ExerciseId == exerciseToUpdate.ExerciseId);
            if (exToUpdate == null) throw new Exception("Exercise to edit not found - from UpdateExercise");

            exToUpdate.AmountOfReps = exerciseToUpdate.AmountOfReps;
            exToUpdate.AmountOfSets = exerciseToUpdate.AmountOfSets;
            exToUpdate.DurationInMinutes = exerciseToUpdate.DurationInMinutes;
            exToUpdate.ExerciseName = exerciseToUpdate.ExerciseName;
            exToUpdate.ImageUri = exerciseToUpdate.ImageUri;
            exToUpdate.Intensity = exerciseToUpdate.Intensity;
            exToUpdate.OtherInfo = exerciseToUpdate.OtherInfo;

            db.SubmitChanges();
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

            db.Exercises.DeleteOnSubmit(exToDelete);

            db.ExTrDayConnectors.DeleteAllOnSubmit(
                db.ExTrDayConnectors.Where(x =>
                    x._exerciseId == exerciseToDelete.ExerciseId));

            db.SubmitChanges(); 
        }

        public void DeleteTrainingsByDate(DateTime dateToWhichDelete)
        {
            var trainingsToDelete = db.TrainingDays.Where(x => x.StartTime < dateToWhichDelete);

            foreach (var item in trainingsToDelete)
            {
                db.ExTrDayConnectors.DeleteAllOnSubmit(
                    db.ExTrDayConnectors.Where(x =>
                        x._trainingDayId == item.TrainingDayId));
            }

            db.TrainingDays.DeleteAllOnSubmit(trainingsToDelete);

            db.SubmitChanges();
        }

        public void DeleteTraining(TrainingDay trainingToDelete)
        {
            TrainingDay trToDelete = GetTrainingById(trainingToDelete.TrainingDayId);
            if (trToDelete == null) throw new Exception("Training to delete not found - from DeleteTraining");

            db.TrainingDays.DeleteOnSubmit(trToDelete);

            db.ExTrDayConnectors.DeleteAllOnSubmit(
                db.ExTrDayConnectors.Where(x =>
                    x._trainingDayId == trainingToDelete.TrainingDayId));

            db.SubmitChanges();
        }
        

        private void DeleteOldTrainings(TimeSpan youngestAgeOfTrainingToDelete)
        {
            var toDelete = db.TrainingDays.Where(x =>
                (x.DurationInMinutes.HasValue
                    ? x.StartTime + TimeSpan.FromSeconds(x.DurationInMinutes.Value)
                    : x.StartTime) < (DateTime.Now - youngestAgeOfTrainingToDelete));

            foreach (var item in toDelete)
            {
                db.ExTrDayConnectors.DeleteAllOnSubmit(
                    db.ExTrDayConnectors.Where(x =>
                        x._trainingDayId == item.TrainingDayId));
            }

            db.TrainingDays.DeleteAllOnSubmit(toDelete);

            db.SubmitChanges(); 
        }

        public void CopyTraining()
        {
            throw new NotImplementedException();
        }

        public void UpdateTraining(TrainingDay trainingToUpdate)
        {
            TrainingDay trToUpdate = GetTrainingById(trainingToUpdate.TrainingDayId);
            if (trToUpdate == null) throw new Exception("Training to edit not found - from UpdateTraining");

            db.TrainingDays.DeleteOnSubmit(trToUpdate);

            db.ExTrDayConnectors.DeleteAllOnSubmit(
                db.ExTrDayConnectors.Where(x =>
                    x._trainingDayId == trainingToUpdate.TrainingDayId));

            trainingToUpdate.TrainingDayId = 0;
            db.TrainingDays.InsertOnSubmit(trainingToUpdate);

            db.SubmitChanges(); 
        }

        public void AddNewTraining(TrainingDay newTraining)
        {
            db.TrainingDays.InsertOnSubmit(newTraining);
            db.SubmitChanges(); 
        }

        public Dictionary<DateTime, int> GetNumberOfExercisesPerDayByMonth(DateTime month)
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
    }
}