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

namespace FitAndGym.View
{
    public partial class TrainingCopyActionsPage : PhoneApplicationPage
    {
        private bool isLoaded = false;
        private TrainingDay trainingDay = null;

        public TrainingCopyActionsPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            var saveChangesButton = new ApplicationBarIconButton(new Uri("/Images/check.png", UriKind.RelativeOrAbsolute));

            saveChangesButton.Click += saveChangesButton_Click;
            saveChangesButton.Text = AppResources.ProceedCloningAppBar;

            ApplicationBar.Buttons.Add(saveChangesButton);
        }

        void saveChangesButton_Click(object sender, EventArgs e)
        {
            if (cloneSwitch.IsChecked.HasValue ? cloneSwitch.IsChecked.Value : false)
                proceedCloning();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                string trainingId;
                int Id;
                if (NavigationContext.QueryString.TryGetValue("trainingId", out trainingId))
                {
                    if (int.TryParse(trainingId, out Id))
                    {
                        trainingDay = App.FitAndGymViewModel.GetTrainingById(Id);
                    }
                }
            }
        }

        private void proceedCloning()
        {
            int frequency = int.Parse(txtBlockFrequencyValue.Text);
            DateTime endDate = EndDateOfWritingTrainings.Value.Value;
            int result;
            TimeSpan dif;

            if ((result = ((dif = endDate - trainingDay.StartTime.Date).Days / frequency)) <= 30)
            {

            }

            //NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=afterCloning&PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
        }

        private void intervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isLoaded)
                txtBlockFrequencyValue.Text = Math.Round(intervalSlider.Value).ToString();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

    }
}