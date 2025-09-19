using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace AMFormsCST.Desktop.Services;

/// <summary>
/// Managed host of the application.
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUpdateManagerService _updateManagerService;
    private readonly ISupportTool _supportTool;
    private readonly ILogService? _logger;

    public ApplicationHostService(
        IServiceProvider serviceProvider,
        IUpdateManagerService updateManagerService,
        ISupportTool supportTool,
        ILogService? logger = null)
    {
        _serviceProvider = serviceProvider;
        _updateManagerService = updateManagerService;
        _supportTool = supportTool;
        _logger = logger;
        _logger?.LogInfo("ApplicationHostService initialized.");
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger?.LogInfo("ApplicationHostService starting.");
        return HandleActivationAsync();
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger?.LogInfo("ApplicationHostService stopping. Saving all settings.");
        _supportTool.SaveAllSettings();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Creates main window during activation.
    /// </summary>
    private async Task HandleActivationAsync()
    {
        try
        {
            _logger?.LogInfo("Checking for updates on startup.");

            if (Application.Current is null ||
                Application.Current.Windows.OfType<MainWindow>().Any())
            {
                _logger?.LogDebug("MainWindow already exists or Application.Current is null.");
                return;
            }

            IWindow mainWindow = _serviceProvider.GetRequiredService<IWindow>();
            mainWindow.Loaded += OnMainWindowLoaded;
            mainWindow?.Show();
            _logger?.LogInfo("MainWindow shown.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error during HandleActivationAsync.", ex);
        }
    }

    private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is not MainWindow mainWindow)
            {
                _logger?.LogWarning("OnMainWindowLoaded called but sender is not MainWindow.");
                return;
            }

            _ = mainWindow.NavigationView.Navigate(typeof(DashboardPage));
            _logger?.LogInfo("Navigated to DashboardPage.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error during OnMainWindowLoaded.", ex);
        }
    }
}
