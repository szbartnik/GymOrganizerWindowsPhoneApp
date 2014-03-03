using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitAndGym.Models;

namespace FitAndGym.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private const int NUM_OF_INCOMING_TRAININGS = 6;

        private ObservableCollection<TrainingDay> _trainings;
        public ObservableCollection<TrainingDay> Trainings
        {
            get { return _trainings; }
            set
            {
                _trainings = value;
                NotifyPropertyChanged("Trainings");
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

        private ObservableCollection<TrainingDay> _incomingTrainings;
        public ObservableCollection<TrainingDay> IncomingTrainings
        {
            get { return _incomingTrainings; }
            set
            {
                _incomingTrainings = value;
                NotifyPropertyChanged("IncomingTrainings");
            }
        }

        public MainPageViewModel()
        {
            Trainings = new ObservableCollection<TrainingDay>();
            IncomingTrainings = new ObservableCollection<TrainingDay>();
            Exercises = new ObservableCollection<Exercise>();

            App.FitAndGymDBMethods.GetTrainingsFromDatabase().ToList().ForEach(x => Trainings.Add(x));
            App.FitAndGymDBMethods.GetTrainingsFromDatabase(NUM_OF_INCOMING_TRAININGS).ToList().ForEach(x => IncomingTrainings.Add(x));
            App.FitAndGymDBMethods.LoadExercisesCollectionFromDatabase().ToList().ForEach(x => Exercises.Add(x));
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
