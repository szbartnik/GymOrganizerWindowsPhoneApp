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

            DataContext = App.FitAndGymViewModel;
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

        #region Events Stuff

        private void TrainingDaysList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TrainingDaysList.SelectedItem is TrainingDay)
            {
                var trainingToShow = TrainingDaysList.SelectedItem as TrainingDay;
                Dispatcher.BeginInvoke(() => MessageBox.Show(trainingToShow.ToString()));
            }
        }

        private void EditTrainingContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var trainingToEdit = (sender as MenuItem).DataContext as TrainingDay;
            NavigationService.Navigate(new Uri("/View/AddNewTrainingPage.xaml?action=edit&trId=" + trainingToEdit.TrainingDayId.ToString(), UriKind.RelativeOrAbsolute));
        }

        private void ExercisesList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ExercisesList.SelectedItem is Exercise)
            {
                var exerciseToShow = ExercisesList.SelectedItem as Exercise;
                Dispatcher.BeginInvoke(() => MessageBox.Show(exerciseToShow.ToString()));
            }
        }

        private void EditExerciseContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var exerciseToEdit = (sender as MenuItem).DataContext as Exercise;
            NavigationService.Navigate(new Uri("/View/AddNewExercisePage.xaml?action=edit&exId=" + exerciseToEdit.ExerciseId.ToString(), UriKind.RelativeOrAbsolute));
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

        void addNewExerciseButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AddNewExercisePage.xaml?action=add", UriKind.Relative));
        }

        void addNewTrainingButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AddNewTrainingPage.xaml?action=add", UriKind.Relative));
        }

        private void DeleteTrainingContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var trainingToDelete = (sender as MenuItem).DataContext as TrainingDay;
            App.FitAndGymViewModel.DeleteTraining(trainingToDelete);
        }

        private void DeleteExerciseContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var exerciseToDelete = (sender as MenuItem).DataContext as Exercise;
            App.FitAndGymViewModel.DeleteExercise(exerciseToDelete);
        }
        
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (MessageBox.Show(
                    AppResources.ConfirmationExitActionAlertContent,
                    AppResources.ConfirmationExitActionAlertHeader,
                    MessageBoxButton.OKCancel) == MessageBoxResult.OK
                   )
                        Application.Current.Terminate();
            });
            e.Cancel = true;
        }

        #endregion
    }
}