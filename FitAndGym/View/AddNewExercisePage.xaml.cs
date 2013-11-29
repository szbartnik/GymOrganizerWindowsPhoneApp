using System;
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
    public partial class AddNewExercisePage : PhoneApplicationPage
    {
        private ExercisePageViewModel _viewModel;

        public AddNewExercisePage()
        {
            InitializeComponent();

            // filling ListPicker by enums
            NewExIntensityListPicker.ItemsSource = Enum.GetValues(typeof(Intensity));
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
                    throw new Exception(String.Format("Unknown action: '{0}' in ExercisePage", action));

                ApplicationBar.Buttons.Add(saveChangesButton);
                ApplicationBar.Buttons.Add(discardChangesButton);
                return;
            }
            throw new Exception("Lack of action in NavigationContext.QueryString in ExercisePage");
        }

        private void updateChanges_Click(object sender, EventArgs e)
        {
            var exerciseToUpdate = _viewModel.GenerateModel();
            if (exerciseToUpdate != null)
            {
                App.FitAndGymViewModel.UpdateExercise(exerciseToUpdate);
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=updatedExercise", UriKind.RelativeOrAbsolute));
            }
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
                NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=addedNewExercise", UriKind.RelativeOrAbsolute));
            }
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
                    string exIdStr;
                    int exId;

                    if (NavigationContext.QueryString.TryGetValue("exId", out exIdStr) && Int32.TryParse(exIdStr, out exId))
                    {
                        Exercise exToEdit = App.FitAndGymViewModel.GetExerciseById(exId);
                        if (exToEdit != null)
                            _viewModel = new ExercisePageViewModel(exToEdit);
                        else
                            throw new Exception(String.Format("Not found Exercise with id = {0} in database invoked from ExercisePage!", exId));
                    }
                    else
                        throw new Exception("Wrong NavigationContext.QueryString 'exId' in ExercisePage");
                }
                else if (action == "add")
                    _viewModel = new ExercisePageViewModel();
                else
                    throw new Exception(String.Format("Wrong NavigationContext.QueryString (action) in ExercisePage. Action = '{0}'", action));

                _viewModel.ValidationError += _viewModel_ValidationError;
                DataContext = _viewModel;
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

        private void NewExName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;
            BindingExpression bindingExpression = txtbox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }
    }
}