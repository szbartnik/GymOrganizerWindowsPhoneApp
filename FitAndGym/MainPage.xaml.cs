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

namespace FitAndGym
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            App.FitAndGymViewModel.LoadTrainingDaysCollectionFromDatabase();
            DataContext = App.FitAndGymViewModel.TrainingDays;
        }
    }
}