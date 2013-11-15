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
    public enum Intensity
    {
        VeryLow,
        Low, 
        Medium, 
        High,
        VeryHigh
    }

    [Table]
    public class Exercise : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _exerciseId;
        private string _exerciseName;
        private Nullable<int> _amountOfSeries;
        private int _durationInMinutes;
        private Intensity _intensity;
        private string _otherInfo;
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
        public int? AmountOfSeries
        {
            get { return _amountOfSeries; }
            set
            {
                if (_amountOfSeries != value)
                {
                    NotifyPropertyChanging("AmountOfSeries");
                    _amountOfSeries = value;
                    NotifyPropertyChanged("AmountOfSeries");
                }
            }
        }

        [Column(CanBeNull = true)]
        public int DurationInMinutes
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
