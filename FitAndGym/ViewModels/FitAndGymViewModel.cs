using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FitAndGym.Models;

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

        #endregion

        public void LoadTrainingDaysCollectionFromDatabase()
        {
            var trainingDaysInDB = from TrainingDay trDay
                                   in db.TrainingDays
                                   select trDay;

            TrainingDays = new ObservableCollection<TrainingDay>(trainingDaysInDB);
        }

        public void LoadExercisesCollectionFromDatabase()
        {
            var exercisesInDB = from Exercise ex
                                in db.Exercises
                                select ex;

            Exercises = new ObservableCollection<Exercise>(exercisesInDB);
        }

        #region Events Stuff

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
