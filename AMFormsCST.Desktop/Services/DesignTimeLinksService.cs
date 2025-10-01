using AMFormsCST.Core.Interfaces;
using System.Diagnostics;

namespace AMFormsCST.Desktop.Services
{
    public class DesignTimeLinksService : ILinksService
    {
        public void OpenLink(string linkKey)
        {
            // In design time, we don't want to open actual links.
            // We can write to the debug console to simulate the action.
            Debug.WriteLine($"[DESIGN TIME] Attempted to open link with key: {linkKey}");
        }
    }
}