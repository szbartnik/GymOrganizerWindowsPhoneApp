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
        private ApplicationBar _exercisesApplicationBar = null;
        private ApplicationBar _trainingsApplicationBar = null;

        public MainPage()
        {
            BuildLocalizedApplicationBar();
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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = _trainingsApplicationBar;
                    break;
                case 1:
                    ApplicationBar = _exercisesApplicationBar;
                    break;
            }
        }

        private void BuildLocalizedApplicationBar()
        {
            //
            // ApplicationBar for Trainings Pivot Item

            _trainingsApplicationBar = new ApplicationBar();

            var addNewTrainingButton = new ApplicationBarIconButton(new Uri("/Images/add.png", UriKind.RelativeOrAbsolute));
            addNewTrainingButton.Click += addNewTrainingButton_Click;
            addNewTrainingButton.Text = AppResources.AddTrainingAppBar;
            _trainingsApplicationBar.Buttons.Add(addNewTrainingButton);

            //
            // ApplicationBar for Exercises Pivot Item

            _exercisesApplicationBar = new ApplicationBar();

            var addNewExerciseButton = new ApplicationBarIconButton(new Uri("/Images/add.png", UriKind.RelativeOrAbsolute));
            addNewExerciseButton.Click += addNewExerciseButton_Click;
            addNewExerciseButton.Text = AppResources.AddExerciseAppBar;
            _exercisesApplicationBar.Buttons.Add(addNewExerciseButton);
        }

        void addNewExerciseButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AddNewExercisePage.xaml", UriKind.Relative));
        }

        void addNewTrainingButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AddNewTrainingPage.xaml", UriKind.Relative));
        }
    }
}