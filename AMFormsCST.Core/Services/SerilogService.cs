using Serilog;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Core.Services
{
    public class SerilogService : ILogService
    {
        private readonly ILogger _logger;

        public SerilogService(ILogger logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message) => _logger.Debug(message);
        public void LogInfo(string message) => _logger.Information(message);
        public void LogWarning(string message) => _logger.Warning(message);
        public void LogError(string message, Exception? ex = null) => _logger.Error(ex, message);
        public void LogFatal(string message, Exception? ex = null) => _logger.Fatal(ex, message);
    }
}