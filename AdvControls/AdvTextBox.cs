using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdvControls
{
    public class AdvTextBox : TextBox
    {
        public static readonly DependencyProperty DefaultTextProperty = DependencyProperty.Register("DefaultText", typeof(string), typeof(AdvTextBox), new PropertyMetadata(""));

        public string DefaultText
        {
            get
            {
                return (string)GetValue(DefaultTextProperty);
            }
            set
            {
                SetValue(DefaultTextProperty, value);
                SetDefaultText();
            }
        }

        public AdvTextBox()
        {
            this.GotFocus += (sender, e) =>
            { if (this.Text.Equals(DefaultText)) { this.Text = String.Empty; } };
            this.LostFocus += (sender, e) => { SetDefaultText(); };
            this.Loaded += (sender, e) => { SetDefaultText(); };
        }

        private void SetDefaultText()
        {
            if (this.Text.Trim().Length == 0)
            {
                this.Text = DefaultText;
            }
        }
    }
}
