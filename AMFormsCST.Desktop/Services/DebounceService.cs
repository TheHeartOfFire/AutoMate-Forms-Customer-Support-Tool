using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Services
{
    public class DebounceService(int debounceInterval = 5000, ILogService? logger = null) : IDebounceService
    {
        public int DebounceIntervalMs => debounceInterval;
        public event EventHandler? DebouncedElapsed;
        private System.Timers.Timer? _debounceTimer;

        private void OnDebouncedElapsed() => DebouncedElapsed?.Invoke(this, EventArgs.Empty);

        public void ScheduleEvent()
        {
            if (_debounceTimer == null)
            {
                _debounceTimer = new System.Timers.Timer(DebounceIntervalMs);
                _debounceTimer.Elapsed += (s, e) =>
                {
                    _debounceTimer?.Stop();
                    _debounceTimer?.Dispose();
                    _debounceTimer = null;
                    OnDebouncedElapsed();
                    logger?.LogInfo("Autosave triggered.");
                };
                _debounceTimer.AutoReset = false;
            }
            else
            {
                _debounceTimer.Stop();
            }
            _debounceTimer.Start();
        }
    }
}
