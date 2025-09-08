namespace AMFormsCST.Desktop.Services;

public interface IUpdateManagerService
{
    Task CheckForUpdatesAsync();
    Task CheckForUpdatesOnStartupAsync();
}