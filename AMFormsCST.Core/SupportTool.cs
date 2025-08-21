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

    public SupportTool(IFileSystem fileSystem, IFormNameBestPractice formNameBestPractice, ISettings defaultSettings, ITemplateRepository templateRepository)
    {
        // Load Properties from config file, or use defaults if not found
        _properties = IO.LoadConfig() ?? new Properties();

        CodeBlocks = new CodeBlocks();
        Notebook = new Notebook();
        FormgenUtils = new FormgenUtils(fileSystem, _properties.FormgenUtils);
        Enforcer = new BestPracticeEnforcer(formNameBestPractice, templateRepository);

        // Load saved settings, or use the default ones if no file exists
        Settings = IO.LoadSettings() ?? defaultSettings;
        Settings.UserSettings.Organization.InstantiateVariables(Enforcer, Notebook);
    }

    public void SaveAllSettings()
    {
        IO.SaveSettings(Settings);
        IO.SaveConfig(_properties);
    }
}
