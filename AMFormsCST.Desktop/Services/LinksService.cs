using System;
using System.Diagnostics;
using AMFormsCST.Core.Interfaces;

namespace AMFormsCST.Desktop.Services
{
    public class LinksService : ILinksService
    {
        private readonly ILogService? _logger;

        public LinksService(ILogService? logger = null)
        {
            _logger = logger;
        }

        public void OpenLink(string linkKey)
        {
            try
            {
                string path = linkKey switch
                {
                    "ADP" => "https://my.adp.com",
                    "SalesForce" => "https://am.my.salesforce.com",
                    "MyApps" => "https://myapps.microsoft.com",
                    "ClientInfoReport" => "https://americamp.sharepoint.com/:x:/r/sites/Compliance-Public/Shared%20Documents/Call%20Center/Client%20Information%20Report.xlsm?d=w2f7b6a8f3c9c4d6a9b7f8e7d0b4a9a6b&csf=1&web=1&e=VvWw9T",
                    "FormsTracker" => "https://americamp.sharepoint.com/sites/Compliance-Public/Lists/Formgen%20Request%20Tracker/AllItems.aspx",
                    "Workday" => "https://wd5.myworkday.com/americamp/d/home.htmld",
                    "CST" => $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\AmpsSupport\\CSTLoader.exe",
                    "Formgen" => $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\NewFormGen.lnk",
                    _ => string.Empty
                };

                if (string.IsNullOrEmpty(path)) return;

                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Failed to open link for {linkKey}.", ex);
            }
        }
    }
}