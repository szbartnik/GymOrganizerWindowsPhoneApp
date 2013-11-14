using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitAndGym.Models
{
    public class TrainingDay : INotifyPropertyChanging, INotifyPropertyChanged
    {
        // Private members
        private int _trainingDayId;
        private string _trainingDayName;
        private DateTime _startTime;
        private TimeSpan _duration;
        private Nullable<decimal> _hydration;
        private string _otherInfo;
        private EntitySet<ExTrDayConn> _exConns;

        public TrainingDay()
        {
            _exConns = new EntitySet<ExTrDayConn>(
                new Action<ExTrDayConn>(this.attach_ExTrDay),
                new Action<ExTrDayConn>(this.detach_ExTrDay)
             );
        }

        private void attach_ExTrDay(ExTrDayConn exTrDayConn)
        {
            NotifyPropertyChanging("ExTrDayConn");
            exTrDayConn._trainingDayId = this._trainingDayId;
        }

        private void detach_ExTrDay(ExTrDayConn exTrDayConn)
        {
            NotifyPropertyChanging("ExTrDayConn");
            //ExConns[1].
        }

        [Association(Storage = "_exercises", OtherKey = "_trainingDay", ThisKey = "TrainingDayId")]
        public EntitySet<ExTrDayConn> ExConns
        {
            get { return this._exConns; }
            set { this._exConns.Assign(value); }
        }

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
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    NotifyPropertyChanging("Duration");
                    _duration = value;
                    NotifyPropertyChanged("Duration");
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

        #region Events Stuff

        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
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
