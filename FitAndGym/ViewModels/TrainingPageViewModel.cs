using System;
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
        private const int INIT_DURATION_IN_MIN = 8;
        private const int MIN_COMMON = 1;

        #endregion

        #region Private variables

        private string _pageTitle;
        private bool _isEditingModeActive;
        private int _trainingId;
        private string _trName;
        private string _otherInfo;

        #endregion

        public TrainingPageViewModel()
        {
            _pageTitle = AppResources.ExercisePageTitleNewMode;
            _isEditingModeActive = false;
            _trName = String.Empty;
            _otherInfo = String.Empty;
        }

        public TrainingPageViewModel(Exercise exercise)
        {
            _pageTitle = AppResources.ExercisePageTitleEditMode;
            _isEditingModeActive = true;
            _trainingId = exercise.ExerciseId;
            _trName = exercise.ExerciseName;
            _otherInfo = exercise.OtherInfo;
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

        #endregion

        public TrainingDay GenerateModel()
        {
            var training = new TrainingDay();

            training.TrainingDayId = _isEditingModeActive ? _trainingId : default(int);

            if (TrName != AppResources.TypeNameOfTrainingPlaceholder && TrName != String.Empty)
                training.TrainingDayName = TrName;
            else
                training.TrainingDayName = String.Empty;
            training.OtherInfo = OtherInfo != AppResources.NewExerciseOtherInfoPlaceholder
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
            str.AppendLine(String.Format("Other info: {0}", _otherInfo));

            return str.ToString();
        }
    }
}
