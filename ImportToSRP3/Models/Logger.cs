using System;
using System.Windows.Forms;

namespace ImportToSRP3.Models
{
    public class Logger:ILogger
    {
        private readonly TextBox _txtLog;

        public Logger(TextBox txtLog)
        {
            this._txtLog = txtLog;
        }

        public void Log(string text)
        {
            _txtLog.AppendText(text);
        }
        public void LogEnd(string text)
        {
            _txtLog.AppendText(text + Environment.NewLine);
        }
    }
}
