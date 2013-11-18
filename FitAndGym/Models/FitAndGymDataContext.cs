using System;
using System.ComponentModel;
using System.Data.Linq;

namespace FitAndGym.Models
{
    public class FitAndGymDataContext : DataContext
    {
        public FitAndGymDataContext(string connectionString)
            : base(connectionString) { }

        public Table<TrainingDay> TrainingDays;
        public Table<Exercise> Exercises;
        public Table<ExTrDayConn> ExTrDayConnectors;
    }
}
