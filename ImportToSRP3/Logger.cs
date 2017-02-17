using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportToSRP3
{
    public class Logger
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
