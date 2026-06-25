
namespace final_work
{
    internal class Log
    {
        //Class that defines that logdetails save username, message and date and time
        private const string LogFolderPath = "c:\\HR-System";
        private const string LogFilePath = LogFolderPath + "\\LogDetails.txt";
        public static string UserName { get; set; }
        public static void LogDetail(string message)
        {
            EnsureLogFolderExists();

            string username = Log.UserName;
            string logEntry = $"User: {username} - {message} - {DateTime.Now}";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
        private static void EnsureLogFolderExists()
        {
            //Ensure that folder exists
            if (!Directory.Exists(LogFolderPath))
            {
                Directory.CreateDirectory(LogFolderPath);
            }
        }
    }
}