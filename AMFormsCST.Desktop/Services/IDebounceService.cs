namespace AMFormsCST.Desktop.Services;

public interface IDebounceService
{
    int DebounceIntervalMs { get; }

    event EventHandler? DebouncedElapsed;

    void ScheduleEvent();
}