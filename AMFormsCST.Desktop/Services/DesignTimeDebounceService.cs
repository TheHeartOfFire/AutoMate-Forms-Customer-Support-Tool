using AMFormsCST.Core.Interfaces;
using System;

namespace AMFormsCST.Desktop.Services;

public class DesignTimeDebounceService : IDebounceService
{
    public int DebounceIntervalMs => -1;

    public event EventHandler? DebouncedElapsed;

    public void ScheduleEvent()
    {
        // No-op for design time
    }

    public void Dispose()
    {
        // No-op for design time
    }
}