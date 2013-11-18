using System;
using System.Windows;
using System.Windows.Controls;
using FitAndGym.Models;
using FitAndGym.Resources;
using FitAndGym.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FitAndGym.View
{
    public partial class AddNewExercisePage : PhoneApplicationPage
    {
        private ExercisePageViewModel _viewModel;

        public AddNewExercisePage()
        {
            BuildLocalizedApplicationBar();
            InitializeComponent();

            // filling ListPicker by enums
            NewExIntensityListPicker.ItemsSource = Enum.GetValues(typeof(Intensity));

            _viewModel = new ExercisePageViewModel();
            _viewModel.ValidationError += _viewModel_ValidationError;
            DataContext = _viewModel;
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            var saveChangesButton = new ApplicationBarIconButton(new Uri("/Images/save.png", UriKind.RelativeOrAbsolute));
            saveChangesButton.Click += saveChanges_Click;
            saveChangesButton.Text = AppResources.SaveChangesAppBar;
            ApplicationBar.Buttons.Add(saveChangesButton);

            var discardChangesButton = new ApplicationBarIconButton(new Uri("/Images/cancel.png", UriKind.RelativeOrAbsolute));
            discardChangesButton.Click += discardChangesButton_Click;
            discardChangesButton.Text = AppResources.DiscardChangesAppBar;
            ApplicationBar.Buttons.Add(discardChangesButton);
        }

        private void discardChangesButton_Click(object sender, EventArgs e)
        {
            _viewModel = null;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            var newExercise = _viewModel.GenerateModel();
            if (newExercise != null)
            {
                App.FitAndGymViewModel.AddNewExercise(newExercise);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        void _viewModel_ValidationError(object sender, ViewModels.ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorMessage, AppResources.ValidationErrorTitle, MessageBoxButton.OK);
        }

        #region Plus-Minus Of Sets&Reps Events

        private void NewExNumOfSetsPlus_Click(object sender, RoutedEventArgs e)
        {
            ++(_viewModel.NumOfSets);
        }

        private void NewExNumOfSetsMinus_Click(object sender, RoutedEventArgs e)
        {
            --(_viewModel.NumOfSets);
        }

        private void NewExNumOfRepsPlus_Click(object sender, RoutedEventArgs e)
        {
            ++(_viewModel.NumOfReps);
        }

        private void NewExNumOfRepsMinus_Click(object sender, RoutedEventArgs e)
        {
            --(_viewModel.NumOfReps);
        }

        #endregion

        #region CheckBox's Activation Events

        private void NewExIntensityActiveCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
                NewExIntensityListPicker.IsEnabled = true;
            else
                NewExIntensityListPicker.IsEnabled = false;
        }

        private void NewExDurationActiveCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
                NewExDurationTimeSpanPicker.IsEnabled = true;
            else
                NewExDurationTimeSpanPicker.IsEnabled = false;
        }

        private void NewExSetsActiveCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                NewExNumOfSetsTextBox.IsEnabled = true;
                NewExNumOfSetsPlus.IsEnabled = true;
                NewExNumOfSetsMinus.IsEnabled = true;
            }
            else
            {
                NewExNumOfSetsTextBox.IsEnabled = false;
                NewExNumOfSetsPlus.IsEnabled = false;
                NewExNumOfSetsMinus.IsEnabled = false;
            }
        }

        private void NewExDurationRepsCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                NewExNumOfRepsTextBox.IsEnabled = true;
                NewExNumOfRepsPlus.IsEnabled = true;
                NewExNumOfRepsMinus.IsEnabled = true;
            }
            else
            {
                NewExNumOfRepsTextBox.IsEnabled = false;
                NewExNumOfRepsPlus.IsEnabled = false;
                NewExNumOfRepsMinus.IsEnabled = false;
            }
        }

        #endregion
    }
}