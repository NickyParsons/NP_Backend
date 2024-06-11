namespace TestAspNetApplication.FileLogger
{
    public class FileLogger : ILogger, IDisposable
    {
        string _logDirectory;
        string _categoryName;
        object _lock;
        public FileLogger(string logDirectory, string categoryName)
        {
            _lock = new object();
            _logDirectory = logDirectory;
            _categoryName = categoryName;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        public void Dispose()
        {

        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                string logFileName = Path.Combine(_logDirectory, _categoryName + '_' + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");
                lock (_lock)
                {
                    File.AppendAllText(logFileName, $"{DateTime.Now.ToString("yyyy.MM.dd H:mm:dd")} [{logLevel}] [{eventId}] | {formatter(state, exception)}" + Environment.NewLine);
                }
            }
        }
    }
}
