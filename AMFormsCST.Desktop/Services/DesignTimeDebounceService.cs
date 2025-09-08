namespace AMFormsCST.Desktop.Services;

public class DesignTimeDebounceService : IDebounceService
{
    public int DebounceIntervalMs => -1;

    public event EventHandler? DebouncedElapsed;

    public void ScheduleEvent() { }

    public void Dispose() { }
}