using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FitAndGym.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FitAndGym.View
{
    public partial class DeleteTrainingsByDatePage : PhoneApplicationPage
    {
        public DeleteTrainingsByDatePage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            var proceedDeleteButton = new ApplicationBarIconButton(new Uri("/Images/delete.png", UriKind.RelativeOrAbsolute));
            var discardChangesButton = new ApplicationBarIconButton(new Uri("/Images/cancel.png", UriKind.RelativeOrAbsolute));

            proceedDeleteButton.Click += proceedDelete_Click;
            proceedDeleteButton.Text = AppResources.ProceedDeleteByDateAppBar;

            discardChangesButton.Click += discardChangesButton_Click;
            discardChangesButton.Text = AppResources.DiscardChangesAppBar;

            ApplicationBar.Buttons.Add(proceedDeleteButton);
            ApplicationBar.Buttons.Add(discardChangesButton);
        }

        void discardChangesButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
        }

        void proceedDelete_Click(object sender, EventArgs e)
        {
            if (DateToWhichDelete != null && MessageBox.Show(AppResources.OlderThan + " " + DateToWhichDelete.Value.Value.Date.ToLongDateString(), AppResources.DatesOfTrainingsThatWillBeDeleted, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                App.FitAndGymViewModel.DeleteTrainingsByDate(DateToWhichDelete.Value.Value);

            NavigationService.Navigate(new Uri("/MainPage.xaml?viewBag=afterCloning&PivotMain.SelectedIndex=1", UriKind.RelativeOrAbsolute));
        }
    }
}