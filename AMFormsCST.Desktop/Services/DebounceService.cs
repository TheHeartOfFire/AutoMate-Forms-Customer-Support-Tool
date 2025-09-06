using AMFormsCST.Core;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
