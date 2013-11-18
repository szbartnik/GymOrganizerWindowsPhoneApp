using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace FitAndGym.Models
{
    [Table]
    public class TrainingDay : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _trainingDayId;
        private string _trainingDayName;
        private DateTime _startTime;
        private Nullable<int> _durationInMinutes;
        private Nullable<decimal> _hydration;
        private string _otherInfo;
        private EntitySet<ExTrDayConn> _exConns;

        #region Ordinary properties

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false)]
        public int TrainingDayId
        {
            get { return _trainingDayId; }
            set
            {
                if (_trainingDayId != value)
                {
                    NotifyPropertyChanging("TrainingDayId");
                    _trainingDayId = value;
                    NotifyPropertyChanged("TrainingDayId");
                }
            }
        }
        
        [Column(CanBeNull = true)]
        public string TrainingDayName
        {
            get { return _trainingDayName; }
            set
            {
                if (_trainingDayName != value)
                {
                    NotifyPropertyChanging("TrainingDayName");
                    _trainingDayName = value;
                    NotifyPropertyChanged("TrainingDayName");
                }
            }
        }

        [Column(CanBeNull = true)]
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    NotifyPropertyChanging("StartTime");
                    _startTime = value;
                    NotifyPropertyChanged("StartTime");
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
        public decimal? Hydration
        {
            get { return _hydration; }
            set
            {
                if (_hydration != value)
                {
                    NotifyPropertyChanging("Hydration");
                    _hydration = value;
                    NotifyPropertyChanged("Hydration");
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

        #endregion

        #region Association stuff + constructor

        [Association(Storage = "_exConns", OtherKey = "_trainingDayId", ThisKey = "TrainingDayId")]
        public EntitySet<ExTrDayConn> ExConns
        {
            get { return this._exConns; }
            set { this._exConns.Assign(value); }
        }

        public TrainingDay()
        {
            _exConns = new EntitySet<ExTrDayConn>();
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
