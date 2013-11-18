using System;

namespace FitAndGym.ViewModels
{
    public delegate void ValidationErrorEventHandler(object sender, ValidationErrorEventArgs e);

    interface IValidableModel<T>
    {
        T GenerateModel();
        event ValidationErrorEventHandler ValidationError;
        string ToString();
    }

    public class ValidationErrorEventArgs
    {
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public ValidationErrorEventArgs(string errorMessage)
        {
            _errorMessage = errorMessage;
        }
    }
}
