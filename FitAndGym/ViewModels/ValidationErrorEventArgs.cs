using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitAndGym.ViewModels
{
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
