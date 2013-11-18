using System;
using System.Text;

namespace FitAndGym.Utilities
{
    public class DebugTextWriter : System.IO.TextWriter
    {
        public override void Write(char[] buffer, int index, int count)
        {
            System.Diagnostics.Debug.WriteLine(new String(buffer, index, count));
        }

        public override void Write(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
