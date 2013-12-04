using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FitAndGym.Models;
using FitAndGym.Resources;
using FitAndGym.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FitAndGym.View
{
    public partial class AddNewTrainingPage : PhoneApplicationPage
    {
        private TrainingPageViewModel _viewModel;

        public AddNewTrainingPage()
        {
            InitializeComponent();
            SetWidthOfGridWithExercisesDependingOnQuantityOfItems();
        }

        private void SetWidthOfGridWithExercisesDependingOnQuantityOfItems()
        {
            ExercisesListGrid.Width = ((App.FitAndGymViewModel.Exercises.Count / 3) + 1) * 270;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // I have to commemorate guy who saved me - http://samondotnet.blogspot.com/2011/12/onnavigatedto-will-be-called-after.html
            // Line of rescue:
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back) return;

            BuildLocalizedApplicationBar();
            CheckIfEditOrAddActionRequired();
        }

        private void CheckIfEditOrAddActionRequired()
        {
            string action;

            if (NavigationContext.QueryString.TryGetValue("action", out action))
            {
                if (action == "edit")
                {
                    string trIdStr;
                    int trId;

                    if (NavigationContext.QueryString.TryGetValue("trId", out trIdStr) && Int32.TryParse(trIdStr, out trId))
                    {
                        TrainingDay trToEdit = App.FitAndGymViewModel.GetTrainingById(trId);
                        if (trToEdit != null)
                        {
                            _viewModel = new TrainingPageViewModel(trToEdit);

                            // adding exercises list to the control
                            ListOfExercises.SelectedItems.Clear();
                            ICollection<Exercise> items = _viewModel.Exercises;

                            foreach (object item in items)
                                ListOfExercises.SelectedItems.Add(item);
                        }
                        else
                            throw new Exception(String.Format("Not found Training with id = {0} in database invoked from TrainingPage!", trId));
                    }
                    else
                        throw new Exception("Wrong NavigationContext.QueryString 'trId' in TrainingPage");
                }
                else if (action == "add")
                    _viewModel = new TrainingPageViewModel();
                else
                    throw new Exception(String.Format("Wrong NavigationContext.QueryString (action) in TrainingPage. Action = '{0}'", action));

                _viewModel.ValidationError += _viewModel_ValidationError;
                DataContext = _viewModel;
            }
        }

        private void _viewModel_ValidationError(object sender, ViewModels.ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorMessage, AppResources.ValidationErrorTitle, MessageBoxButton.OK);
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            var saveChangesButton = new ApplicationBarIconButton(new Uri("/Images/save.png", UriKind.RelativeOrAbsolute));
            var discardChangesButton = new ApplicationBarIconButton(new Uri("/Images/cancel.png", UriKind.RelativeOrAbsolute));

            string action;

            if (NavigationContext.QueryString.TryGetValue("action", out action))
            {
                if (action == "edit")
                {
                    saveChangesButton.Click += updateChanges_Click;
                    saveChangesButton.Text = AppResources.UpdateChangesAppBar;
                    discardChangesButton.Click += discardChangesButton_Click;
                    discardChangesButton.Text = AppResources.DiscardChangesAppBar;
                }
                else if (action == "add")
                {
                    saveChangesButton.Click += saveChanges_Click;
                    saveChangesButton.Text = AppResources.SaveChangesAppBar;
                    discardChangesButton.Click += discardChangesButton_Click;
                    discardChangesButton.Text = AppResources.DiscardChangesAppBar;
                }
                else
                    throw new Exception(String.Format("Unknown action: '{0}' in TrainingPage", action));

                ApplicationBar.Buttons.Add(saveChangesButton);
                ApplicationBar.Buttons.Add(discardChangesButton);
                return;
            }
            throw new Exception("Lack of action in NavigationContext.QueryString in TrainingPage");
        }

        private void updateChanges_Click(object sender, EventArgs e)
        {
            var trainingToUpdate = _viewModel.GenerateModel();
            if (trainingToUpdate != null)
            {
                App.FitAndGymViewModel.UpdateTraining(trainingToUpdate);
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=updatedTraining", UriKind.RelativeOrAbsolute));
            }
        }

        private void discardChangesButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            var newTraining = _viewModel.GenerateModel();
            if (newTraining != null)
            {
                App.FitAndGymViewModel.AddNewTraining(newTraining);
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=addedNewTraining", UriKind.RelativeOrAbsolute));
            }
        }

        private void NewTrName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;
            BindingExpression bindingExpression = txtbox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }

        private void ListOfExercises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedExercise = e.AddedItems[0] as Exercise;
                _viewModel.SelectedExercises.Add(selectedExercise);
            }
            else if (e.RemovedItems.Count > 0)
            {
                var unselectedExercise = e.RemovedItems[0] as Exercise;
                _viewModel.SelectedExercises.Remove(unselectedExercise);
            }
        }

        private void NewTrHydrationPlus_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Hydration += 0.1M;
        }

        private void NewTrHydrationMinus_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Hydration -= 0.1M;
        }
    }
}