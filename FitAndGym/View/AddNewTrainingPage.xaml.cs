using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
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
        private TrainingDay _trToEdit;

        public AddNewTrainingPage()
        {
            InitializeComponent();
        }

        private async Task CheckIfEditOrAddActionRequiredAsync()
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
                        _trToEdit = App.FitAndGymViewModel.GetTrainingById(trId);
                        if (_trToEdit != null)
                        {
                            await Task.Factory.StartNew(() => _viewModel = new TrainingPageViewModel(_trToEdit));
                            DataContext = _viewModel;

                        }
                        else
                            throw new Exception(String.Format("Not found Training with id = {0} in database invoked from TrainingPage!", trId));
                    }
                    else
                        throw new Exception("Wrong NavigationContext.QueryString 'trId' in TrainingPage");
                }
                else if (action == "add")
                {
                    await Task.Factory.StartNew(() => _viewModel = new TrainingPageViewModel());
                    DataContext = _viewModel;
                }
                else
                    throw new Exception(String.Format("Wrong NavigationContext.QueryString (action) in TrainingPage. Action = '{0}'", action));

                _viewModel.ValidationError += _viewModel_ValidationError;
            }
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            var saveChangesButton = new ApplicationBarIconButton(new Uri("/Images/save.png", UriKind.RelativeOrAbsolute));
            var discardChangesButton = new ApplicationBarIconButton(new Uri("/Images/cancel.png", UriKind.RelativeOrAbsolute));
            var helpButton = new ApplicationBarIconButton(new Uri("/Images/questionmark.png", UriKind.RelativeOrAbsolute));

            string action;

            if (NavigationContext.QueryString.TryGetValue("action", out action))
            {
                if (action == "edit")
                {
                    saveChangesButton.Click += updateChanges_Click;
                    saveChangesButton.Text = AppResources.UpdateChangesAppBar;
                }
                else if (action == "add")
                {
                    saveChangesButton.Click += saveChanges_Click;
                    saveChangesButton.Text = AppResources.SaveChangesAppBar;
                }
                else
                    throw new Exception(String.Format("Unknown action: '{0}' in TrainingPage", action));

                discardChangesButton.Click += discardChangesButton_Click;
                discardChangesButton.Text = AppResources.DiscardChangesAppBar;

                helpButton.Click += helpButton_Click;
                helpButton.Text = AppResources.HelpInTrainingPageAppBar;

                ApplicationBar.Buttons.Add(saveChangesButton);
                ApplicationBar.Buttons.Add(helpButton);
                ApplicationBar.Buttons.Add(discardChangesButton);
                return;
            }
            throw new Exception("Lack of action in NavigationContext.QueryString in TrainingPage");
        }

        #region Events Stuff

        private void updateChanges_Click(object sender, EventArgs e)
        {
            var trainingToUpdate = _viewModel.GenerateModel();
            if (trainingToUpdate != null)
            {
                App.FitAndGymViewModel.UpdateTraining(trainingToUpdate);
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=updatedTraining&PivotMain.SelectedIndex=0", UriKind.RelativeOrAbsolute));
            }
        }

        private void discardChangesButton_Click(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            var newTraining = _viewModel.GenerateModel();
            if (newTraining != null)
            {
                App.FitAndGymViewModel.AddNewTraining(newTraining);
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=addedTraining&PivotMain.SelectedIndex=0", UriKind.RelativeOrAbsolute));
            }
        }

        void helpButton_Click(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() => MessageBox.Show(AppResources.HelpContentInManageTraining, AppResources.HelpHeaderInManageTraining, MessageBoxButton.OK));
        }

        private void NewTrHydrationPlus_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Hydration += 0.1M;
        }

        private void NewTrHydrationMinus_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Hydration -= 0.1M;
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

        private void _viewModel_ValidationError(object sender, ViewModels.ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorMessage, AppResources.ValidationErrorTitle, MessageBoxButton.OK);
        }

        protected override async void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // I have to commemorate guy who saved me - http://samondotnet.blogspot.com/2011/12/onnavigatedto-will-be-called-after.html
            // Line of rescue:
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back) return;

            BuildLocalizedApplicationBar();
            await CheckIfEditOrAddActionRequiredAsync();
        }

        private void NewTrName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;
            BindingExpression bindingExpression = txtbox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            if (_trToEdit != null)
            {
                foreach (ExTrDayConn conn in _trToEdit.ExConns)
                    ListOfExercises.SelectedItems.Add(conn.Exercise);
            }
        }

        private void NewTrOthersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AdvControls.AdvTextBox txtbox = sender as AdvControls.AdvTextBox;
            BindingExpression bindingExpression = txtbox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }

        #endregion
    }
}