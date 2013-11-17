using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FitAndGym.Infrastructure;
using FitAndGym.Models;
using FitAndGym.Resources;
using FitAndGym.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FitAndGym.View
{
    public partial class AddNewExercisePage : PhoneApplicationPage
    {
        private const int MAX_NUM_OF_SETS = 100;
        private const int MAX_NUM_OF_REPS = 500;
        private const int INIT_NUM_OF_SETS = 5;
        private const int INIT_NUM_OF_REPS = 15;
        private const int MIN_COMMON = 1;

        private AddNewExercisePageViewModel _viewModel;

        public AddNewExercisePage()
        {
            BuildLocalizedApplicationBar();
            InitializeComponent();

            // filling ListPicker
            NewExIntensityListPicker.ItemsSource = Enum.GetValues(typeof(Intensity));

            _viewModel = new AddNewExercisePageViewModel();
            ControlsStartValuesInitialization();
            DataContext = _viewModel;
        }

        private void ControlsStartValuesInitialization()
        {
            _viewModel.NumOfSets = INIT_NUM_OF_SETS;
            _viewModel.NumOfReps = INIT_NUM_OF_REPS;
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
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            // TODO


            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        #region Plus-Minus Of Sets&Reps Events

        private void NewExNumOfSetsPlus_Click(object sender, RoutedEventArgs e)
        {
            if(_viewModel.NumOfSets < MAX_NUM_OF_SETS)
                ++(_viewModel.NumOfSets);
        }

        private void NewExNumOfSetsMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.NumOfSets > MIN_COMMON)
                --(_viewModel.NumOfSets);
        }

        private void NewExNumOfRepsPlus_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.NumOfReps < MAX_NUM_OF_REPS)
                ++(_viewModel.NumOfReps);
        }

        private void NewExNumOfRepsMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.NumOfReps > MIN_COMMON)
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