using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Services;

public class UpdateManagerService : IUpdateManagerService
{
    private readonly ISnackbarService _snackbarService;
    private readonly UpdateManager _updateManager;

    public UpdateManagerService(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
        // Initialize the UpdateManager with your GitHub repository source.
        _updateManager = new UpdateManager(new GithubSource("https://github.com/TheHeartOfFire/AutoMate-Forms-Customer-Support-Tool", null, false));
    }

    public async Task CheckForUpdatesAsync()
    {
        try
        {
            var newVersion = await _updateManager.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                _snackbarService.Show("No Updates Found", "Your application is up to date.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Checkmark24), TimeSpan.FromSeconds(3));
                return;
            }

            await DownloadAndApplyUpdate(newVersion);
        }
        catch (Exception ex)
        {
            _snackbarService.Show("Update Check Failed", ex.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle24), TimeSpan.FromSeconds(5));
        }
    }

    public async Task CheckForUpdatesOnStartupAsync()
    {
        try
        {
            var newVersion = await _updateManager.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                return; // No updates, exit silently.
            }

            await DownloadAndApplyUpdate(newVersion);
        }
        catch
        {
            // Suppress exceptions on startup check to avoid interrupting the user.
        }
    }

    private async Task DownloadAndApplyUpdate(UpdateInfo newVersion)
    {
        try
        {
            _snackbarService.Show("Update Available", $"Downloading version {newVersion.TargetFullRelease.Version}...", ControlAppearance.Info, new SymbolIcon(SymbolRegular.ArrowDownload24), TimeSpan.FromSeconds(5));

            await _updateManager.DownloadUpdatesAsync(newVersion);

            _updateManager.ApplyUpdatesAndRestart(newVersion);
        }
        catch (Exception ex)
        {
            _snackbarService.Show("Update Failed", ex.Message, ControlAppearance.Danger, new SymbolIcon(SymbolRegular.ErrorCircle24), TimeSpan.FromSeconds(5));
        }
    }
}

