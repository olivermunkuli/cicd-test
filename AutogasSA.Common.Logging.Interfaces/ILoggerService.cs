namespace AutogasSA.Common.Logging.Interfaces
{
    public interface ILoggerService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? ex);
        void LogDebug(string message);
    }
}