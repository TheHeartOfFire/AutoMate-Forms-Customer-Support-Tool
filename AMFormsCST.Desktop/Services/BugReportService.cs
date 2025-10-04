using AMFormsCST.Core.Interfaces;
using AMFormsCST.Desktop.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace AMFormsCST.Desktop.Services;


public class BugReportService : IBugReportService
{
    private readonly ILogService? _logger;
    private readonly IDialogService _dialogService;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public BugReportService(ILogService? logger, IDialogService dialogService, IConfiguration configuration)
    {
        _logger = logger;
        _dialogService = dialogService;
        _configuration = configuration;
        // Set Authorization header per instance, as it may depend on configuration
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration[Properties.Resources.LimitedGHPat]);
    }

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AMFormsCST", GetAppVersion()));
        return client;
    }
    public async Task CreateBugReportAsync()
    {
        var (result, title, description) = _dialogService.ShowBugReportDialog();

        if (!result || string.IsNullOrWhiteSpace(title))
        {
            _logger?.LogInfo("Bug report creation cancelled by user.");
            return;
        }

        var confirmation = _dialogService.ShowMessageBox(
            "This will submit your bug report to GitHub. Are you sure you want to proceed?",
            "Confirm Bug Report",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirmation != MessageBoxResult.Yes)
        {
            _logger?.LogInfo("Bug report submission cancelled by user at confirmation.");
            return;
        }

        try
        {
            // Step 1: Create the Gist directly from the client.
            string? gistUrl = await UploadLogAsGistAsync();
            if (gistUrl == null)
            {
                _dialogService.ShowMessageBox("Could not upload logs to create a Gist. Aborting bug report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Step 2: Trigger the GitHub Action with a small payload.
            var dispatchUrl = $"https://api.github.com/repos/{Properties.Resources.GitHubRepoOwner}/{Properties.Resources.GitHubRepoName}/dispatches";
            var encodedDescription = Convert.ToBase64String(Encoding.UTF8.GetBytes(description));

            var payload = new
            {
                event_type = "create-bug-report",
                client_payload = new
                {
                    title,
                    description_base64 = encodedDescription,
                    gist_url = gistUrl, // Send the URL, not the content
                    app_version = GetAppVersion(),
                    os_version = RuntimeInformation.OSDescription
                }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(dispatchUrl, content);

            if (response.IsSuccessStatusCode)
            {
                _dialogService.ShowMessageBox("Bug report submitted successfully! It will appear on GitHub shortly.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _logger?.LogInfo("Successfully triggered the bug report GitHub Action.");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _dialogService.ShowMessageBox($"Failed to submit bug report. Status: {response.StatusCode}\n{errorContent}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger?.LogError($"Failed to trigger GitHub Action. Status: {response.StatusCode}, Response: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _dialogService.ShowMessageBox($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _logger?.LogError("An exception occurred while creating a bug report.", ex);
        }
    }

    private async Task<string?> UploadLogAsGistAsync()
    {
        try
        {
            var logContent = await GetLogContentAsync();
            if (string.IsNullOrEmpty(logContent))
            {
                _logger?.LogWarning("Log content is empty, skipping Gist creation.");
                return "No logs available.";
            }

            var gist = new GitHubGist
            {
                Description = $"AMFormsCST Log File - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC",
                Public = false,
                Files = new Dictionary<string, GistFile>
                {
                    ["log.txt"] = new() { Content = logContent }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(gist);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.github.com/gists", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);
                var htmlUrl = doc.RootElement.GetProperty("html_url").GetString();
                _logger?.LogInfo($"Successfully created Gist at {htmlUrl}");
                return htmlUrl;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger?.LogError($"Failed to create Gist. Status: {response.StatusCode}, Response: {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError("An exception occurred while creating a Gist.", ex);
            return null;
        }
    }

    private async Task<string> GetLogContentAsync()
    {
        try
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
            if (!Directory.Exists(logDirectory)) return "Log directory not found.";

            var logFile = Directory.GetFiles(logDirectory, "app*.log")
                                   .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                                   .FirstOrDefault();

            if (logFile is null) return "No log files found.";

            using var fileStream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(fileStream);
            return await streamReader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError("Failed to read log files.", ex);
            return $"Error reading logs: {ex.Message}";
        }
    }

    private static string GetAppVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "1.0.0";
}