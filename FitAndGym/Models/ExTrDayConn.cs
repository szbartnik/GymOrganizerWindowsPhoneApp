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
    [Table]
    public class ExTrDayConn : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _Id;
        private EntityRef<Exercise> _exercise;
        private EntityRef<TrainingDay> _trainingDay;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false)]
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    NotifyPropertyChanging("Id");
                    _Id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        //
        // exercise

        [Column]
        internal int _exerciseId;

        [Association(Storage = "_exercise", ThisKey = "_exerciseId", OtherKey = "ExerciseId", IsForeignKey = true)]
        public Exercise Exercise
        {
            get { return _exercise.Entity; }
            set
            {
                NotifyPropertyChanging("Exercise");
                _exercise.Entity = value;

                if (value != null)
                {
                    _exerciseId = value.ExerciseId;
                }
                NotifyPropertyChanged("Exercise");
            }
        }

        //
        // training day

        [Column]
        internal int _trainingDayId;

        [Association(Storage = "_trainingDay", ThisKey = "_trainingDayId", OtherKey = "TrainingDayId", IsForeignKey = true)]
        public TrainingDay TrainingDay
        {
            get { return _trainingDay.Entity; }
            set
            {
                NotifyPropertyChanging("TrainingDay");
                _trainingDay.Entity = value;

                if (value != null)
                {
                    _trainingDayId = value.TrainingDayId;
                }
                NotifyPropertyChanged("TrainingDay");
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
