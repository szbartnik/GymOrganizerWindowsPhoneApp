using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using FitAndGym.Infrastructure;

namespace FitAndGym.Models
{
    public enum Intensity
    {
        [LocalizedDescription("IntensityVeryLow")]
        VeryLow,
        [LocalizedDescription("IntensityLow")]
        Low,
        [LocalizedDescription("IntensityMedium")]
        Medium,
        [LocalizedDescription("IntensityHigh")]
        High,
        [LocalizedDescription("IntensityVeryHigh")]
        VeryHigh
    }

    [Table]
    public class Exercise : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _exerciseId;
        private string _exerciseName;
        private Nullable<int> _amountOfSets;
        private Nullable<int> _amountOfReps;
        private Nullable<int> _durationInMinutes;
        private Intensity _intensity;
        private string _otherInfo;
        private string _imageUri;
        private EntitySet<ExTrDayConn> _exConns;

        #region Ordinary properties

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false)]
        public int ExerciseId
        {
            get { return _exerciseId; }
            set
            {
                if (_exerciseId != value)
                {
                    NotifyPropertyChanging("ExerciseId");
                    _exerciseId = value;
                    NotifyPropertyChanged("ExerciseId");
                }
            }
        }

        [Column(CanBeNull = false)]
        public string ExerciseName
        {
            get { return _exerciseName; }
            set
            {
                if (_exerciseName != value)
                {
                    NotifyPropertyChanging("ExerciseName");
                    _exerciseName = value;
                    NotifyPropertyChanged("ExerciseName");
                }
            }
        }

        [Column(CanBeNull = true)]
        public int? AmountOfSets
        {
            get { return _amountOfSets; }
            set
            {
                if (_amountOfSets != value)
                {
                    NotifyPropertyChanging("AmountOfSets");
                    _amountOfSets = value;
                    NotifyPropertyChanged("AmountOfSets");
                }
            }
        }

        [Column(CanBeNull = true)]
        public int? AmountOfReps
        {
            get { return _amountOfReps; }
            set
            {
                if (_amountOfReps != value)
                {
                    NotifyPropertyChanging("AmountOfReps");
                    _amountOfReps = value;
                    NotifyPropertyChanged("AmountOfReps");
                }
            }
        }

        [Column(CanBeNull = true)]
        public int? DurationInMinutes
        {
            get { return _durationInMinutes; }
            set
            {
                if (_durationInMinutes != value)
                {
                    NotifyPropertyChanging("DurationInMinutes");
                    _durationInMinutes = value;
                    NotifyPropertyChanged("DurationInMinutes");
                }
            }
        }

        [Column(CanBeNull = true)]
        public Intensity Intensity
        {
            get { return _intensity; }
            set
            {
                if (_intensity != value)
                {
                    NotifyPropertyChanging("Intensity");
                    _intensity = value;
                    NotifyPropertyChanged("Intensity");
                }
            }
        }

        [Column(CanBeNull = true)]
        public string OtherInfo
        {
            get { return _otherInfo; }
            set
            {
                if (_otherInfo != value)
                {
                    NotifyPropertyChanging("OtherInfo");
                    _otherInfo = value;
                    NotifyPropertyChanged("OtherInfo");
                }
            }
        }

        [Column(CanBeNull = true)]
        public string ImageUri
        {
            get { return _imageUri; }
            set
            {
                if (_imageUri != value)
                {
                    NotifyPropertyChanging("ImageUri");
                    _imageUri = value;
                    NotifyPropertyChanged("ImageUri");
                }
            }
        }

        #endregion

        #region Association stuff + constructor

        public Exercise()
        {
            _exConns = new EntitySet<ExTrDayConn>();
        }

        [Association(Storage = "_exConns", OtherKey = "_exerciseId", ThisKey = "ExerciseId")]
        public EntitySet<ExTrDayConn> ExConns
        {
            get { return this._exConns; }
            set { this._exConns.Assign(value); }
        }

        #endregion

        #region Events Stuff

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
