using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FitAndGym.Resources;
using Microsoft.Phone.Controls;

namespace FitAndGym.Utilities
{
    public static class AboutMessageBox
    {
        public static void ShowAboutAppMessageBox(string createdByLine, string additionalContentLine)
        {
            var firstLineInMessageBox = new TextBlock()
            {
                Margin = new Thickness(10, 0, 0, 0),
                Text = createdByLine
            };

            var secondLineInMessageBox = new TextBlock()
            {
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Margin = new Thickness(10, 10, 20, 0),
                Text = additionalContentLine
            };

            var hyperlinkButton = new HyperlinkButton()
            {
                Content = AppResources.CreatorEmail,
                Margin = new Thickness(0, 28, 0, 8),
                HorizontalAlignment = HorizontalAlignment.Left,
                NavigateUri = new Uri("mailto://" + AppResources.CreatorEmail, UriKind.Absolute)
            };

            var contentStackPanel = new StackPanel();
            contentStackPanel.Children.Add(firstLineInMessageBox);
            contentStackPanel.Children.Add(hyperlinkButton);
            contentStackPanel.Children.Add(secondLineInMessageBox);

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = String.Format("{0} {1} v{2}.{3}\nbuild {4}", AppResources.AboutSentence, AppResources.ApplicationTitle, version.Major, version.Minor, version.Build),
                Content = contentStackPanel,
                LeftButtonContent = "OK"
            };
            messageBox.Show();
        }
    }
}
