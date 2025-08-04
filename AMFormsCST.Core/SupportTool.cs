using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Types.UserSettings;
using AMFormsCST.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core;
public class SupportTool : ISupportTool
{
    public IBestPracticeEnforcer Enforcer { get; set; }
    public ICodeBlocks CodeBlocks { get; set; }
    public IFormgenUtils FormgenUtils { get; set; }
    public INotebook Notebook { get; set; }
    public ISettings Settings { get; set; }
    private readonly Properties _properties = new();


    public SupportTool(IFormNameBestPractice formNameBestPractice)
    {
        _properties = new Properties();
        CodeBlocks = new CodeBlocks();
        Notebook = new Notebook();
        FormgenUtils = new FormgenUtils(_properties.FormgenUtils);
        Enforcer = new BestPracticeEnforcer(formNameBestPractice);
        Settings = new Settings(
            new UserSettings(
                new AutomateFormsOrgVariables(Enforcer, Notebook)));

    }
}
