using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitAndGym.ViewModels
{
    public class AddNewExercisePageViewModel : INotifyPropertyChanged
    {
        public AddNewExercisePageViewModel()
        {

        }

        private int _numOfReps;
        public int NumOfReps
        {
            get { return _numOfReps; }
            set
            {
                _numOfReps = value;
                NotifyPropertyChanged("NumOfReps");
            }
        }

        private int _numOfSets;
        public int NumOfSets
        {
            get { return _numOfSets; }
            set
            {
                _numOfSets = value;
                NotifyPropertyChanged("NumOfSets");
            }
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
