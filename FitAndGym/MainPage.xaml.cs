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
using Microsoft.Phone.Tasks;
using FitAndGym.Utilities;
using FitAndGym.ViewModels;

namespace FitAndGym
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool actionOfUserToLeaveThePagePerformed = false;
        private ApplicationBar _exercisesApplicationBar = null;
        private ApplicationBar _trainingsApplicationBar = null;
        private ApplicationBar _infoApplicationBar = null;
        private ApplicationBar _calendarApplicationBar = null;
        private CustomMessageBox calendarMessageBox = null;
        private MainPageViewModel _viewModel = null;

        public MainPage()
        {
            BuildLocalizedApplicationBar();
            InitializeComponent();

            _viewModel = new MainPageViewModel();
            DataContext = _viewModel;
        }

        #if DEBUG
        ~MainPage()
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                System.Windows.MessageBox.Show("MainPage Destructing");
                // Seeing this message box assures that this page is being cleaned up
            }));
        }
        #endif

        private void BuildLocalizedApplicationBar()
        {
            //
            // ApplicationBar for Trainings Pivot Item

            _trainingsApplicationBar = new ApplicationBar();

            var addNewTrainingButton = new ApplicationBarIconButton(new Uri("/Images/add.png", UriKind.RelativeOrAbsolute));
            addNewTrainingButton.Click += addNewTrainingButton_Click;
            addNewTrainingButton.Text = AppResources.AddTrainingAppBar;
            _trainingsApplicationBar.Buttons.Add(addNewTrainingButton);

            var deleteOldTrainingsMenuItem = new ApplicationBarMenuItem(AppResources.DeleteOldTrainingsMenuText);
            deleteOldTrainingsMenuItem.Click += deleteOldTrainingsMenuItem_Click;
            _trainingsApplicationBar.MenuItems.Add(deleteOldTrainingsMenuItem);

            //
            // ApplicationBar for Exercises Pivot Item

            _exercisesApplicationBar = new ApplicationBar();

            var addNewExerciseButton = new ApplicationBarIconButton(new Uri("/Images/add.png", UriKind.RelativeOrAbsolute));
            addNewExerciseButton.Click += addNewExerciseButton_Click;
            addNewExerciseButton.Text = AppResources.AddExerciseAppBar;
            _exercisesApplicationBar.Buttons.Add(addNewExerciseButton);

            //
            // ApplicationBar for Incoming Pivot Item

            _infoApplicationBar = new ApplicationBar();

            var infoButton = new ApplicationBarIconButton(new Uri("/Images/info.png", UriKind.RelativeOrAbsolute));
            infoButton.Click += infoButton_Click;
            infoButton.Text = AppResources.InfoAppBar;

            var rateButton = new ApplicationBarIconButton(new Uri("/Images/like.png", UriKind.RelativeOrAbsolute));
            rateButton.Click += rateButton_Click;
            rateButton.Text = AppResources.RateAppBar;

            _infoApplicationBar.Buttons.Add(infoButton);
            _infoApplicationBar.Buttons.Add(rateButton);

            //
            // ApplicationBar for Calendar Pivot Item

            _calendarApplicationBar = new ApplicationBar();

            var helpOfCalendarButton = new ApplicationBarIconButton(new Uri("/Images/questionmark.png", UriKind.RelativeOrAbsolute));
            helpOfCalendarButton.Click += helpOfCalendarButton_Click;
            helpOfCalendarButton.Text = AppResources.HelpButtonInCalendarAppBar;

            _calendarApplicationBar.Buttons.Add(helpOfCalendarButton);
            _calendarApplicationBar.Buttons.Add(infoButton);
            _calendarApplicationBar.Buttons.Add(rateButton);
        }

        void helpOfCalendarButton_Click(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() => MessageBox.Show(AppResources.HelpContentInCalendar, AppResources.HelpHeaderInCalendar, MessageBoxButton.OK));
        }

        #region Events Stuff

        void deleteOldTrainingsMenuItem_Click(object sender, EventArgs e)
        {
            actionOfUserToLeaveThePagePerformed = true;
            NavigationService.Navigate(new Uri("/View/DeleteTrainingsByDatePage.xaml", UriKind.RelativeOrAbsolute));
        }

        void rateButton_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        void infoButton_Click(object sender, EventArgs e)
        {
            string createdByLine = String.Format("\n{0}\n{1}",
                AppResources.DesignedAndCreatedBySentence,
                AppResources.CreatorNameAndSurname);

            AboutMessageBox.ShowAboutAppMessageBox(createdByLine, AppResources.AdditionalContentOfAboutBox);
        }

        private void TrainingDaysList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((sender as LongListSelector).SelectedItem is TrainingDay)
            {
                var listOfExercises = new List<Exercise>();
                var trainingToShow = ((sender as LongListSelector).SelectedItem as TrainingDay).ExConns.ToList();

                trainingToShow.ForEach(x => listOfExercises.Add(x.Exercise));
                listOfExercises.Sort((tr1, tr2) => tr1.ExerciseName.CompareTo(tr2.ExerciseName));

                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.ConnectedExercisesMessageBoxHeader,
                    Message = ((sender as LongListSelector).SelectedItem as TrainingDay).OtherInfo ,
                    Content = listOfExercises,
                    ContentTemplate = (DataTemplate)this.LayoutRoot.Resources["ListOfExercisesOfTrainingTemplate"],
                    LeftButtonContent = "OK"
                };
                messageBox.Show();
            }
        }

        private void EditTrainingContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var trainingToEdit = (sender as MenuItem).DataContext as TrainingDay;
            actionOfUserToLeaveThePagePerformed = true;
            NavigationService.Navigate(new Uri("/View/AddNewTrainingPage.xaml?action=edit&trId=" + trainingToEdit.TrainingDayId.ToString(), UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            while (NavigationService.BackStack.Any())
            {
                NavigationService.RemoveBackEntry();
            }

            if (e.NavigationMode != NavigationMode.Back)
            {
                string s_page;

                if (NavigationContext.QueryString.TryGetValue("PivotMain.SelectedIndex", out s_page))
                {
                    int i_page;

                    if (Int32.TryParse(s_page, out i_page))
                        PivotMain.SelectedIndex = i_page;
                }
            }
        }

        private void ExercisesList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((sender as LongListSelector).SelectedItem is Exercise)
            {
                var exerciseToShow = (sender as LongListSelector).SelectedItem as Exercise;
                if(exerciseToShow.OtherInfo != String.Empty)
                    Dispatcher.BeginInvoke(() => MessageBox.Show(exerciseToShow.OtherInfo, AppResources.OtherInfo2CaptionOnTheMainPage, MessageBoxButton.OK));
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (actionOfUserToLeaveThePagePerformed)
            {
                TrainingDaysList.ClearValue(LongListSelector.ItemsSourceProperty);
                TrainingDaysList.ClearValue(LongListSelector.ItemTemplateProperty);

                ExercisesList.ClearValue(LongListSelector.ItemsSourceProperty);
                ExercisesList.ClearValue(LongListSelector.ItemTemplateProperty);

                IncomingTrainingDaysList.ClearValue(LongListSelector.ItemsSourceProperty);
                IncomingTrainingDaysList.ClearValue(LongListSelector.ItemTemplateProperty);

                DataContext = null;
            }
        }

        private void EditExerciseContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var exerciseToEdit = (sender as MenuItem).DataContext as Exercise;
            actionOfUserToLeaveThePagePerformed = true;
            NavigationService.Navigate(new Uri("/View/AddNewExercisePage.xaml?action=edit&exId=" + exerciseToEdit.ExerciseId.ToString(), UriKind.RelativeOrAbsolute));
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    ApplicationBar = _calendarApplicationBar;
                    await new Task(() => calendarControl.Refresh());
                    break;
                case 1:
                    ApplicationBar = _infoApplicationBar;
                    break;
                case 2:
                    ApplicationBar = _trainingsApplicationBar;
                    break;
                case 3:
                    ApplicationBar = _exercisesApplicationBar;
                    break;
            }
        }

        void addNewExerciseButton_Click(object sender, EventArgs e)
        {
            actionOfUserToLeaveThePagePerformed = true;
            NavigationService.Navigate(new Uri("/View/AddNewExercisePage.xaml?action=add", UriKind.Relative));
        }

        void addNewTrainingButton_Click(object sender, EventArgs e)
        {
            actionOfUserToLeaveThePagePerformed = true;
            NavigationService.Navigate(new Uri("/View/AddNewTrainingPage.xaml?action=add", UriKind.Relative));
        }

        private void DeleteTrainingContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var trainingToDelete = (sender as MenuItem).DataContext as TrainingDay;
            App.FitAndGymDBMethods.DeleteTraining(trainingToDelete);
        }

        private void DeleteExerciseContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var exerciseToDelete = (sender as MenuItem).DataContext as Exercise;
            App.FitAndGymDBMethods.DeleteExercise(exerciseToDelete);
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

        private void CopyTrainingContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var trainingToCopy = (sender as MenuItem).DataContext as TrainingDay;
            actionOfUserToLeaveThePagePerformed = true;

            NavigationService.Navigate(new Uri("/View/TrainingCopyActionsPage.xaml?trainingId=" + trainingToCopy.TrainingDayId, UriKind.RelativeOrAbsolute));
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var menu = (ContextMenu)sender;
            var owner = (FrameworkElement)menu.Owner;
            if (owner.DataContext != menu.DataContext)
                menu.DataContext = owner.DataContext;
        }

        private void calendarControl_DateClicked(object sender, WPControls.SelectionChangedEventArgs e)
        {
            var listOfTrainings = App.FitAndGymDBMethods.GetTrainingsByDate(e.SelectedDate).ToList();

            if (listOfTrainings.Count == 0) return;

            calendarMessageBox = new CustomMessageBox()
            {
                Caption = AppResources.TrainingsOfTheDay,
                Content = listOfTrainings,
                ContentTemplate = (DataTemplate)this.LayoutRoot.Resources["ListOfTrainingsOfDayTemplate"],
                LeftButtonContent = "OK"
            };
            calendarMessageBox.Show();

        }

        private void DeleteTrainingInCalendar_Click(object sender, RoutedEventArgs e)
        {
            var trainingToDelete = (sender as MenuItem).DataContext as TrainingDay;
            App.FitAndGymDBMethods.DeleteTraining(trainingToDelete);
            calendarMessageBox.Dismiss();
            calendarControl.Refresh();
        }
        
        #endregion
    }
}