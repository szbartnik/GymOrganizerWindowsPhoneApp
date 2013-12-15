using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using FitAndGym.Models;
using FitAndGym.Resources;
using FitAndGym.Utilities;
using FitAndGym.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FitAndGym.View
{
    public partial class AddNewTrainingPage : PhoneApplicationPage
    {
        private TrainingPageViewModel _viewModel;
        private TrainingDay _trToEdit;
        private bool isLoaded = false;
        private bool actionOfUserToLeaveThePagePerformed = false;

        public AddNewTrainingPage()
        {
            InitializeComponent();
        }

        #if DEBUG
        ~AddNewTrainingPage()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                System.Windows.MessageBox.Show("AddNewTrainingPage Destructing");
                // Seeing this message box assures that this page is being cleaned up
            }));
        }
        #endif

        private void CheckIfEditOrAddActionRequiredAsync()
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
                            _viewModel = new TrainingPageViewModel(_trToEdit);
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
                    _viewModel = new TrainingPageViewModel();
                    DataContext = _viewModel;
                }
                else
                    throw new Exception(String.Format("Wrong NavigationContext.QueryString (action) in TrainingPage. Action = '{0}'", action));

                _viewModel.ValidationError += _viewModel_ValidationError;
            }
        }

        private void ClearThePage()
        {
            ListOfExercises.ClearValue(LongListMultiSelector.ItemsSourceProperty);
            ListOfExercises.ClearValue(LongListMultiSelector.ItemTemplateProperty);

            DataContext = null;
            _viewModel.ValidationError -= _viewModel_ValidationError;
            _viewModel = null;
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (actionOfUserToLeaveThePagePerformed)
                ClearThePage();
        }

        private void updateChanges_Click(object sender, EventArgs e)
        {
            var trainingToUpdate = _viewModel.GenerateModel();
            if (trainingToUpdate != null)
            {
                actionOfUserToLeaveThePagePerformed = true;
                App.FitAndGymViewModel.UpdateTraining(trainingToUpdate);
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=updatedTraining&PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
            }
        }

        private void discardChangesButton_Click(object sender, EventArgs e)
        {
            actionOfUserToLeaveThePagePerformed = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml?PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            discardChangesButton_Click(this, e);
        }

        private void saveChanges_Click(object sender, EventArgs e)
        {
            var newTraining = _viewModel.GenerateModel();
            if (newTraining != null)
            {
                actionOfUserToLeaveThePagePerformed = true;
                App.FitAndGymViewModel.AddNewTraining(newTraining);
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=addedTraining&PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
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
            if (isLoaded)
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
        }

        private void _viewModel_ValidationError(object sender, ViewModels.ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorMessage, AppResources.ValidationErrorTitle, MessageBoxButton.OK);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            while (NavigationService.BackStack.Any())
            {
                NavigationService.RemoveBackEntry();
            }

            // I have to commemorate guy who saved me - http://samondotnet.blogspot.com/2011/12/onnavigatedto-will-be-called-after.html
            // Line of rescue:
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.Back) return;

            BuildLocalizedApplicationBar();
            CheckIfEditOrAddActionRequiredAsync();
        }

        private void NewTrName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;
            BindingExpression bindingExpression = txtbox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = false;
            if (_trToEdit != null)
            {
                foreach (var item in _viewModel.SelectedExercises)
                {
                    var container = ListOfExercises.ContainerFromItem(item)
                                          as LongListMultiSelectorItem;
                    if (container != null) container.IsSelected = true;
                }
            }
            isLoaded = true;
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