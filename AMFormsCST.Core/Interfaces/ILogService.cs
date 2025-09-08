namespace AMFormsCST.Core.Interfaces
{
    public interface ILogService
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? ex = null);
        void LogFatal(string message, Exception? ex = null);
    }
}