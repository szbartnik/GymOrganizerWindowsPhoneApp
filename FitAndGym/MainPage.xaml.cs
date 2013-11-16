using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FitAndGym.Resources;
using FitAndGym.Models;
using System.Text;
using System.Threading.Tasks;

namespace FitAndGym
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            App.FitAndGymViewModel.LoadTrainingDaysCollectionFromDatabase();
            App.FitAndGymViewModel.LoadExercisesCollectionFromDatabase();
            DataContext = App.FitAndGymViewModel;
        }

        private async void TrainingDaysList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TrainingDaysList.SelectedItem is TrainingDay)
            {
                var selectedTrainingDay = TrainingDaysList.SelectedItem as TrainingDay;
                
                var str = new StringBuilder();

                await Task.Run(() => selectedTrainingDay.ExConns.ToList().ForEach(x => str.AppendLine(x.Exercise.ExerciseName)));
                Dispatcher.BeginInvoke(() => MessageBox.Show(str.ToString()));
            }
        }

        private async void ExercisesList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ExercisesList.SelectedItem is Exercise)
            {
                var selectedExercise = ExercisesList.SelectedItem as Exercise;

                var str = new StringBuilder();

                await Task.Run(() => selectedExercise.ExConns.ToList().ForEach(x => str.AppendLine(x.TrainingDay.TrainingDayName)));
                Dispatcher.BeginInvoke(() => MessageBox.Show(str.ToString()));
            }
        }

    }
}