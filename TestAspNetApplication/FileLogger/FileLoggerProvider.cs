namespace TestAspNetApplication.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        string path;
        public FileLoggerProvider(string logFilePath)
        {
            path = logFilePath;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path, categoryName);
        }

        public void Dispose()
        {

        }
    }
}
