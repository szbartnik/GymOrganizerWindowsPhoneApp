using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void ShowAboutAppMessageBox(string applicationTitle, string firstLine, string creatorName, string mail, string aboutSentence)
        {
            var firstLineInMessageBox = new TextBlock()
            {
                Margin = new Thickness(10, 0, 0, 0),
                Text = firstLine
            };

            var hyperlinkButton = new HyperlinkButton()
            {
                Content = mail,
                Margin = new Thickness(0, 28, 0, 8),
                HorizontalAlignment = HorizontalAlignment.Left,
                NavigateUri = new Uri("mailto://" + mail, UriKind.Absolute)
            };

            var contentStackPanel = new StackPanel();
            contentStackPanel.Children.Add(firstLineInMessageBox);
            contentStackPanel.Children.Add(hyperlinkButton);

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = String.Format("{0} {1}", aboutSentence, applicationTitle),
                Content = contentStackPanel,
                LeftButtonContent = "OK"
            };
            messageBox.Show();
        }
    }
}
