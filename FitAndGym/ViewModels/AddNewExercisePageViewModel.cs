using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FitAndGym.Models;
using FitAndGym.Resources;

namespace FitAndGym.ViewModels
{
    public class AddNewExercisePageViewModel : INotifyPropertyChanged
    {
        private const int MAX_NUM_OF_SETS = 100;
        private const int MAX_NUM_OF_REPS = 500;
        private const int MAX_LENGTH_OF_OTHER_INFO = 400;
        private const int MAX_LENGTH_OF_EXNAME = 60;
        private const int INIT_NUM_OF_SETS = 5;
        private const int INIT_NUM_OF_REPS = 15;
        private const int INIT_DURATION_IN_MIN = 8;
        private const int MIN_COMMON = 1;

        private string _exName;
        private Intensity _intensity;
        private TimeSpan _duration;
        private int _numOfReps;
        private int _numOfSets;
        private string _otherInfo;

        private bool _intensityActive;
        private bool _durationActive;
        private bool _numOfSetsActive;
        private bool _numOfRepsActive;

        public AddNewExercisePageViewModel()
        {
            _numOfReps = INIT_NUM_OF_REPS;
            _numOfSets = INIT_NUM_OF_SETS;
            _exName = String.Empty;
            _otherInfo = String.Empty;
            _intensityActive = true;
            _durationActive = true;
            _numOfSetsActive = true;
            _numOfRepsActive = true;
            _duration = TimeSpan.FromMinutes(INIT_DURATION_IN_MIN);
            _intensity = Intensity.Medium;
        }
 
        public string ExName
        {
            get { return _exName; }
            set
            {
                if (value.Length <= MAX_LENGTH_OF_OTHER_INFO)
                    _exName = value;
                else
                {
                    _exName = value.Substring(0, MAX_LENGTH_OF_EXNAME);
                    NotifyPropertyChanged("ExName");
                }
            }
        }

        public Intensity Intensity
        {
            get { return _intensity; }
            set
            {
                if (value != _intensity)
                    _intensity = value;
            }
        }

        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (value != _duration)
                    _duration = value;
            }
        }

        public int NumOfReps
        {
            get { return _numOfReps; }
            set
            {
                if (value >= MIN_COMMON && value <= MAX_NUM_OF_REPS)
                {
                    _numOfReps = value;
                    NotifyPropertyChanged("NumOfReps");
                }
            }
        }
        
        public int NumOfSets
        {
            get { return _numOfSets; }
            set
            {
                if (value >= MIN_COMMON && value <= MAX_NUM_OF_SETS)
                {
                    _numOfSets = value;
                    NotifyPropertyChanged("NumOfSets");
                }
            }
        }

        public string OtherInfo
        {
            get { return _otherInfo; }
            set
            {
                if (value != _otherInfo)
                {
                    if (value.Length <= MAX_LENGTH_OF_OTHER_INFO)
                        _otherInfo = value;
                    else
                    {
                        _otherInfo = value.Substring(0, MAX_LENGTH_OF_OTHER_INFO);
                    }
                    NotifyPropertyChanged("OtherInfo");
                }
            }
        }


        public bool IntensityActive
        {
            get { return _intensityActive; }
            set { _intensityActive = value; }
        }

        public bool DurationActive
        {
            get { return _durationActive; }
            set { _durationActive = value; }
        }

        public bool NumOfSetsActive
        {
            get { return _numOfSetsActive; }
            set { _numOfSetsActive = value; }
        }

        public bool NumOfRepsActive
        {
            get { return _numOfRepsActive; }
            set { _numOfRepsActive = value; }
        }


        public Exercise GenerateExerciseModel()
        {
            var exercise = new Exercise();

            exercise.ExerciseName = ExName;
            exercise.Intensity = IntensityActive ? Intensity : Intensity.Medium;
            exercise.DurationInMinutes = DurationActive ? (int)Duration.TotalSeconds : (int?)null;
            exercise.AmountOfSets = NumOfSetsActive ? NumOfSets : (int?)null;
            exercise.AmountOfReps = NumOfRepsActive ? NumOfReps : (int?)null;
            exercise.OtherInfo = OtherInfo != AppResources.NewExerciseOtherInfoPlaceholder
                ? OtherInfo
                : String.Empty;
            return exercise;
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

        public override string ToString()
        {
            var str = new StringBuilder();

            str.AppendLine(String.Format("Exercise Name: {0}", _exName));
            str.AppendLine(String.Format("Intensity: {0}, Active: {1}", _intensity, _intensityActive));
            str.AppendLine(String.Format("Duration: {0}, Active: {1}", _duration, _durationActive));
            str.AppendLine(String.Format("Num of sets: {0}, Active: {1}", _numOfSets, _numOfSetsActive));
            str.AppendLine(String.Format("Num of reps: {0}, Active: {1}", _numOfReps, _numOfRepsActive));
            str.AppendLine(String.Format("Other info: {0}", _otherInfo));

            return str.ToString();
        }
    }
}
