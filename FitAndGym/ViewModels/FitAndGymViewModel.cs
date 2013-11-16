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
        private FitAndGymDataContext fitAndGymDB;

        public FitAndGymViewModel(string fitAndGymConnectionString)
        {
            fitAndGymDB = new FitAndGymDataContext(fitAndGymConnectionString);
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
                                   in fitAndGymDB.TrainingDays
                                   select trDay;

            TrainingDays = new ObservableCollection<TrainingDay>(trainingDaysInDB);
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
