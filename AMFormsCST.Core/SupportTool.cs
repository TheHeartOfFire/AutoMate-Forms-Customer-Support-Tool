using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Types.UserSettings;
using AMFormsCST.Core.Utils;

namespace AMFormsCST.Core;
public class SupportTool : ISupportTool
{
    public IBestPracticeEnforcer Enforcer { get; set; }
    public ICodeBlocks CodeBlocks { get; set; }
    public IFormgenUtils FormgenUtils { get; set; }
    public INotebook Notebook { get; set; }
    public ISettings Settings { get; set; }
    private readonly Properties _properties;

    // Add logging service property
    public ILogService? Logger { get; }

    // Add ILogService to constructor
    public SupportTool(
        IFileSystem fileSystem,
        IFormNameBestPractice formNameBestPractice,
        ISettings defaultSettings,
        ITemplateRepository templateRepository,
        INotebook notebook,
        ILogService? logger = null)
    {
        Logger = logger;
        Logger?.LogInfo("Initializing SupportTool.");

        IO.ConfigureLogger(logger);

        // Load Properties from config file, or use defaults if not found
        _properties = IO.LoadConfig() ?? new Properties(logger);

        CodeBlocks = new CodeBlocks(logger);
        Notebook = notebook ?? new Notebook(logger);
        FormgenUtils = new FormgenUtils(fileSystem, _properties.FormgenUtils, logger);
        Enforcer = new BestPracticeEnforcer(formNameBestPractice, templateRepository, logger);

        // Load saved settings, or use the default ones if no file exists
        Settings = IO.LoadSettings() ?? defaultSettings;
        Settings.UserSettings.Organization.InstantiateVariables(Enforcer, Notebook);

        Logger?.LogInfo("SupportTool initialized successfully.");
    }

    public void SaveAllSettings()
    {
        Logger?.LogInfo("Saving all settings.");
        IO.SaveSettings(Settings);
        IO.SaveConfig(_properties);
        Logger?.LogInfo("Settings saved.");
    }
}
