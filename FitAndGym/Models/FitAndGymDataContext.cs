using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
