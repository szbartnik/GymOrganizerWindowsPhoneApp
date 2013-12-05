using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FitAndGym.Models;
using FitAndGym.Resources;

namespace FitAndGym.ViewModels
{
    public class TrainingPageViewModel : INotifyPropertyChanged, IValidableModel<TrainingDay>
    {
        #region Constants 

        private const int MAX_LENGTH_OF_OTHER_INFO = 400;
        private const int MAX_LENGTH_OF_TRNAME = 60;
        private const int INIT_DURATION_IN_MIN = 30;
        private const int MIN_COMMON = 1;
        private const decimal INIT_HYDRATION = 1.5M;
        private const decimal MIN_HYDRATION = 0.1M;
        private const decimal MAX_HYDRATION = 10;

        #endregion

        #region Private variables

        private string _pageTitle;
        private bool _isEditingModeActive;
        private int _trainingId;
        private string _trName;
        private DateTime _startTime;
        private DateTime _startDate;
        private TimeSpan _duration;
        private decimal _hydration;
        private string _otherInfo;

        private bool _hydrationActive;
        private bool _durationActive;

        public ObservableCollection<Exercise> _selectedExercises;

        #endregion

        public ObservableCollection<Exercise> Exercises
        {
            get { return App.FitAndGymViewModel.Exercises; }
        }

        public ObservableCollection<Exercise> SelectedExercises
        {
            get { return _selectedExercises; }
            set
            {
                if (value != _selectedExercises)
                {
                    value = _selectedExercises;
                    NotifyPropertyChanged("SelectedExercises");
                }
            }
        }

        public TrainingPageViewModel()
        {
            _selectedExercises = new ObservableCollection<Exercise>();
            _pageTitle = AppResources.TrainingPageTitleNewMode;
            _startTime = DateTime.Now;
            _startDate = DateTime.Today;
            _isEditingModeActive = false;
            _hydrationActive = true;
            _durationActive = true;
            _trName = String.Empty;
            _duration = TimeSpan.FromMinutes(INIT_DURATION_IN_MIN);
            _hydration = INIT_HYDRATION;
            _otherInfo = String.Empty;
        }

        public TrainingPageViewModel(TrainingDay training)
        {
            _selectedExercises = new ObservableCollection<Exercise>();
            foreach (ExTrDayConn conn in training.ExConns)
                _selectedExercises.Add(conn.Exercise);

            _hydrationActive = training.Hydration.HasValue;
            _durationActive = training.DurationInMinutes.HasValue;
            _startTime = training.StartTime;
            _startDate = training.StartTime;

            _duration = training.DurationInMinutes.HasValue
               ? TimeSpan.FromSeconds(training.DurationInMinutes.Value)
               : TimeSpan.FromMinutes(INIT_DURATION_IN_MIN);

            _hydration = training.Hydration.HasValue ? training.Hydration.Value : INIT_HYDRATION;
            _pageTitle = AppResources.TrainingPageTitleEditMode;
            _isEditingModeActive = true;
            _trainingId = training.TrainingDayId;
            _trName = training.TrainingDayName;
            _otherInfo = training.OtherInfo;
        }

        #region Properties

        public string PageTitle
        {
            get { return _pageTitle; }
            private set { }
        }

        public int TrainingId
        {
            get { return _trainingId; }
            private set { }
        }

        public string TrName
        {
            get { return _trName; }
            set
            {
                if (value.Length <= MAX_LENGTH_OF_OTHER_INFO)
                    _trName = value;
                else
                {
                    _trName = value.Substring(0, MAX_LENGTH_OF_TRNAME);
                    NotifyPropertyChanged("TrName");
                }
            }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (value != _startTime)
                    _startTime = value;
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (value != _startDate)
                    _startDate = value;
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

        public decimal Hydration
        {
            get { return _hydration; }
            set
            {
                if (value >= MIN_HYDRATION && value <= MAX_HYDRATION)
                {
                    _hydration = value;
                    NotifyPropertyChanged("Hydration");
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

        public bool HydrationActive
        {
            get { return _hydrationActive; }
            set
            {
                if (value != _hydrationActive)
                {
                    _hydrationActive = value;
                    NotifyPropertyChanged("HydrationActive");
                }
            }
        }

        public bool DurationActive
        {
            get { return _durationActive; }
            set
            {
                if (value != _durationActive)
                {
                    _durationActive = value;
                    NotifyPropertyChanged("DurationActive");
                }
            }
        }

        #endregion

        public TrainingDay GenerateModel()
        {
            var training = new TrainingDay();



            training.Hydration = HydrationActive ? (decimal)Hydration : (decimal?)null;
            training.DurationInMinutes = DurationActive ? (int)Duration.TotalSeconds : (int?)null;
            training.TrainingDayId = _isEditingModeActive ? _trainingId : default(int);

            if (StartTime != null)
                training.StartTime = StartTime;
            else
            {
                training = null;
                NotifyValidationError(new ValidationErrorEventArgs(AppResources.StartTimeOfTrainingPropertyIsRequiredNotification));
                return null;
            }

            if (TrName != AppResources.TypeNameOfTrainingPlaceholder && TrName != String.Empty)
                training.TrainingDayName = TrName;
            else
                training.TrainingDayName = String.Empty;

            training.OtherInfo = OtherInfo != AppResources.NewTrainingOtherInfoPlaceholder
                ? OtherInfo
                : String.Empty;

            return training;
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

        public event ValidationErrorEventHandler ValidationError;
        private void NotifyValidationError(ValidationErrorEventArgs eventArgs)
        {
            if (ValidationError != null)
            {
                ValidationError(this, eventArgs);
            }
        }
        #endregion

        public override string ToString()
        {
            var str = new System.Text.StringBuilder();

            str.AppendLine(String.Format("Training Name: {0}", _trName));
            str.AppendLine(String.Format("Start time: {0}", _startTime.ToShortDateString()));
            str.AppendLine(String.Format("Duration: {0}", _duration));
            str.AppendLine(String.Format("Hydration: {0}", _hydration.ToString()));
            str.AppendLine(String.Format("Other info: {0}", _otherInfo));

            return str.ToString();
        }
    }
}
