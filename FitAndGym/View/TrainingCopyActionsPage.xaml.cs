using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FitAndGym.Models;
using FitAndGym.Resources;
using System.Text;
using System.Collections.ObjectModel;

namespace FitAndGym.View
{
    public partial class TrainingCopyActionsPage : PhoneApplicationPage
    {
        // I REALLY SORRY FOR MVVM LACK BUT I had REALLY REALLY NO TIME

        private const int MAX_NO_OF_SIM_CLONING_TRAININGS = 30; // change Resources strings also

        private bool isLoaded = false;
        private TrainingDay trainingDay = null;
        private int frequency = 1;

        public DateTime EndSelectedDate
        {
            get
            {
                return EndDateOfWritingTrainings.Value.Value;
            }
        }

        public int NumOfTrainingsThatWillBeAdded
        {
            get
            {
                if (EndSelectedDate <= trainingDay.StartTime.Date.Date)
                    return 0;

                if (EndSelectedDate < DateTime.Today.Date)
                    return 0;

                return ((EndSelectedDate - trainingDay.StartTime.Date).Days / frequency) - (Math.Abs((DateTime.Now - trainingDay.StartTime).Days) / frequency);
            }
        }

        public TrainingCopyActionsPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            var saveChangesButton = new ApplicationBarIconButton(new Uri("/Images/check.png", UriKind.RelativeOrAbsolute));
            var discardChangesButton = new ApplicationBarIconButton(new Uri("/Images/cancel.png", UriKind.RelativeOrAbsolute));

            saveChangesButton.Click += saveChangesButton_Click;
            saveChangesButton.Text = AppResources.ProceedCloningAppBar;

            discardChangesButton.Click += discardChangesButton_Click;
            discardChangesButton.Text = AppResources.DiscardChangesAppBar;

            ApplicationBar.Buttons.Add(saveChangesButton);
            ApplicationBar.Buttons.Add(discardChangesButton);
        }

        void discardChangesButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
        }

        void saveChangesButton_Click(object sender, EventArgs e)
        {
            if (cloneSwitch.IsChecked.HasValue ? cloneSwitch.IsChecked.Value : false)
            {
                proceedCloning();
                return;
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=afterCloning&PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                string trainingId;
                int Id;

                if (NavigationContext.QueryString.TryGetValue("trainingId", out trainingId))
                    if (int.TryParse(trainingId, out Id))
                        trainingDay = App.FitAndGymViewModel.GetTrainingById(Id);
            }
        }

        private bool Validate()
        {
            if (EndSelectedDate <= trainingDay.StartTime.Date.Date)
            {
                NotifyError(AppResources.EndDateCannotBeBeforeStartDate);
                return false;
            }

            if (EndSelectedDate < DateTime.Today.Date)
            {
                NotifyError(AppResources.YouCannotInsertTrainingsInThePast);
                return false;
            }

            if (NumOfTrainingsThatWillBeAdded <= 0)
            {
                NotifyError(AppResources.YouHaveToAddMoreThanZeroClones);
                return false;
            }

            if (NumOfTrainingsThatWillBeAdded >= MAX_NO_OF_SIM_CLONING_TRAININGS)
            {
                NotifyError(AppResources.YouCannotInsertMoreThanxClonesSimu);
                return false;
            }
            return true;
        }

        private void proceedCloning()
        {
            if (Validate())
            {
                var str = new StringBuilder();
                var trainingsToAdd = new Collection<TrainingDay>();
                int i = 0;

                for (DateTime currentDate = trainingDay.StartTime; currentDate <= (EndSelectedDate+TimeSpan.FromDays(1)); currentDate += TimeSpan.FromDays(frequency))
                {
                    if (currentDate > DateTime.Now)
                    {
                        str.AppendLine(currentDate.ToString());
                        trainingsToAdd.Add(trainingDay.Copy().SetDate(currentDate));

                        if(++i == MAX_NO_OF_SIM_CLONING_TRAININGS) break;
                    }
                }

                if (MessageBox.Show(str.ToString(), AppResources.TrainingsThatWillBeAdded, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    foreach (var item in trainingsToAdd)
                        App.FitAndGymViewModel.AddNewTraining(item);

                    NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=afterCloning&PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void NotifyError(string errorMsg)
        {
            Dispatcher.BeginInvoke(() => MessageBox.Show(errorMsg, AppResources.ValidationErrorTitle, MessageBoxButton.OK));
        }

        private void intervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isLoaded)
            {
                txtBlockFrequencyValue.Text = Math.Round(intervalSlider.Value).ToString();
                frequency = int.Parse(txtBlockFrequencyValue.Text);
                UpdateNumOfTrainingsThatWillBeAddedBox();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        private void EndDateOfWritingTrainings_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            UpdateNumOfTrainingsThatWillBeAddedBox();
        }

        private void UpdateNumOfTrainingsThatWillBeAddedBox()
        {
            numOfTrainingsThatWillBeAddedTextBlock.Text = NumOfTrainingsThatWillBeAdded.ToString();
        }
    }
}