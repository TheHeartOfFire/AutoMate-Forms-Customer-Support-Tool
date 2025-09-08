using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Services;

public class WindowsProviderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService? _logger;

    public WindowsProviderService(IServiceProvider serviceProvider, ILogService? logger = null)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _logger?.LogInfo("WindowsProviderService initialized.");
    }

    public void Show<T>()
        where T : class
    {
        try
        {
            if (!typeof(Window).IsAssignableFrom(typeof(T)))
            {
                var ex = new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");
                _logger?.LogError("Attempted to show a non-Window type.", ex);
                throw ex;
            }

            Window windowInstance =
                _serviceProvider.GetService<T>() as Window
                ?? throw new InvalidOperationException("Window is not registered as service.");
            windowInstance.Owner = Application.Current.MainWindow;
            windowInstance.Show();
            _logger?.LogInfo($"Window shown: {typeof(T).Name}");
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Error showing window of type {typeof(T).Name}.", ex);
            throw;
        }
    }
}
