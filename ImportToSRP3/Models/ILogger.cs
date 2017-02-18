namespace ImportToSRP3.Models
{
    public interface ILogger
    {
        //Logs string without end of line
        void Log(string log);
        //Logs the string with end of line character (newline)
        void LogEnd(string log);
    }
}