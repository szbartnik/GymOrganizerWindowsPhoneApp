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

namespace FitAndGym
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBar _exercisesApplicationBar = null;
        private ApplicationBar _trainingsApplicationBar = null;
        private ApplicationBar _infoApplicationBar = null;

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
        }

        #region Events Stuff

        void rateButton_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        void infoButton_Click(object sender, EventArgs e)
        {
            string firstLine = String.Format("{0}\n{1}",
                AppResources.DesignedAndCreatedBySentence,
                AppResources.CreatorNameAndSurname);

            AboutMessageBox.ShowAboutAppMessageBox(
                AppResources.ApplicationTitle,
                firstLine,
                AppResources.CreatorNameAndSurname,
                AppResources.CreatorEmail,
                AppResources.AboutSentence);
        }

        private void TrainingDaysList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TrainingDaysList.SelectedItem is TrainingDay)
            {
                var listOfExercises = new List<Exercise>();
                var trainingToShow = TrainingDaysList.SelectedItem as TrainingDay;
                trainingToShow.ExConns.ToList().ForEach(x => listOfExercises.Add(x.Exercise));

                var longListSelector = new LongListSelector()
                {
                    Margin = new Thickness(0, 10, 0, 0),
                    LayoutMode = LongListSelectorLayoutMode.Grid,
                    ItemsSource = listOfExercises,
                    GridCellSize = new Size(210, 200),
                    ItemTemplate = (DataTemplate)this.LayoutRoot.Resources["ExercisesListTemplate"],
                    Height = 600,
                };
                longListSelector.Tap += ExercisesList_Tap;
                
                    var scroll = new ScrollViewer()
                {
                    Content = longListSelector,
                };

                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.ConnectedExercisesMessageBoxHeader,
                    Content = scroll,
                    DataContext = listOfExercises,
                    LeftButtonContent = "OK",
                };
                messageBox.Show();
            }
        }

        private void EditTrainingContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var trainingToEdit = (sender as MenuItem).DataContext as TrainingDay;
            NavigationService.Navigate(new Uri("/View/AddNewTrainingPage.xaml?action=edit&trId=" + trainingToEdit.TrainingDayId.ToString(), UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string page;
            string action;

            if (NavigationContext.QueryString.TryGetValue("PivotMain.SelectedIndex", out page))
            {
                if      (page == "0") PivotMain.SelectedIndex = 0;
                else if (page == "1") PivotMain.SelectedIndex = 1;
                else if (page == "2") PivotMain.SelectedIndex = 2;
            }

            if (NavigationContext.QueryString.TryGetValue("viewBag", out action))
            {
               // if (action == "addedTraining")

            }
        }

        private void ExercisesList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((sender as LongListSelector).SelectedItem is Exercise)
            {
                var exerciseToShow = ExercisesList.SelectedItem as Exercise;
                if(exerciseToShow.OtherInfo != String.Empty)
                    Dispatcher.BeginInvoke(() => MessageBox.Show(exerciseToShow.OtherInfo, AppResources.OtherInfo2CaptionOnTheMainPage, MessageBoxButton.OK));
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
                    ApplicationBar = _infoApplicationBar;
                    break;
                case 1:
                    ApplicationBar = _trainingsApplicationBar;
                    break;
                case 2:
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