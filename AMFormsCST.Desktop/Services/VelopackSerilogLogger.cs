using AMFormsCST.Core.Interfaces;
using Velopack.Logging;

namespace AMFormsCST.Desktop.Services;

/// <summary>
/// An adapter to connect Velopack's logging to the application's ILogService.
/// </summary>
internal class VelopackSerilogLogger(ILogService logService) : IVelopackLogger
{

    public void Log(VelopackLogLevel logLevel, string? message, Exception? exception)
    {
        if (message is null)
        {
            return;
        }

        switch (logLevel)
        {
            case VelopackLogLevel.Debug:
                    logService.LogDebug(message);
                break;
            case VelopackLogLevel.Information:
                    logService.LogInfo(message);
                break;
            case VelopackLogLevel.Warning:
                    logService.LogWarning(message);
                break;
            case VelopackLogLevel.Error:
                    logService.LogError(message, exception);
                break;
            case VelopackLogLevel.Critical:
                    logService.LogFatal(message, exception);
                break;
        }
    }
}