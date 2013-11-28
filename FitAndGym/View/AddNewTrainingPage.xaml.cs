using System;
using FitAndGym.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FitAndGym.View
{
    public partial class AddNewTrainingPage : PhoneApplicationPage
    {
        public AddNewTrainingPage()
        {
            BuildLocalizedApplicationBar();
            InitializeComponent();

            SetWidthOfGridWithExercisesDependingOnQuantityOfItems();
            DataContext = App.FitAndGymViewModel;
        }

        private void SetWidthOfGridWithExercisesDependingOnQuantityOfItems()
        {
            ExercisesListGrid.Width = ((App.FitAndGymViewModel.Exercises.Count / 3) + 1) * 270;
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
            throw new NotImplementedException();
        }

        private void NewTrName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}