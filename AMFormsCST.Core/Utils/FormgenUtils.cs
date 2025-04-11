using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure;
using System.Xml;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.CodeLineSettings;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Core.Utils;
public class FormgenUtils(FormgenUtilsProperties properties) : IFormgenUtils
{
    public DotFormgen? ParsedFormgenFile { get; private set; }
    public string? FileName => _formgenXml.BaseURI[(_formgenXml.BaseURI.LastIndexOf('\\') + 1)..^8];

    private XmlDocument _formgenXml = new();
    private FormgenUtilsProperties _properties = properties;

    public void OpenFile(string filePath)
    {
        _formgenXml.Load(filePath);
        if (_formgenXml.DocumentElement is null) return;

        ParsedFormgenFile = new DotFormgen(_formgenXml.DocumentElement);

    }
    public void RenameFile(string newName, bool hasImageFile, bool renameImage)
    {
        if (ParsedFormgenFile is null) return;
        if (_formgenXml is null || _formgenXml.BaseURI is null) return;

        var oldName = _formgenXml.BaseURI[(_formgenXml.BaseURI.LastIndexOf('/') + 1)..^8];
        var fileDir = _formgenXml.BaseURI.Replace("file:///", string.Empty);
        fileDir = fileDir[..(fileDir.LastIndexOf('/') + 1)];

        CreateBackup(ParsedFormgenFile.Settings.UUID);

        ParsedFormgenFile.Title = newName;

        var xml = ParsedFormgenFile.GenerateXML();
        if (xml != null) _formgenXml.LoadXml(xml);

        _formgenXml.Save(fileDir + oldName + ".formgen");

        File.Move(fileDir + oldName + ".formgen", fileDir + newName + ".formgen");

        if (hasImageFile && renameImage)
        {
            if (ParsedFormgenFile!.FormType == Format.Pdf)
            {
                File.Move(fileDir + oldName + ".pdf", fileDir + newName + ".pdf");
            }
            else
            {
                File.Move(fileDir + oldName + ".jpg", fileDir + newName + ".jpg");
            }
        }

        _formgenXml.Load(fileDir + newName + ".formgen");

        if (_formgenXml.DocumentElement is null) return;

        ParsedFormgenFile = new DotFormgen(_formgenXml.DocumentElement);
    }
    public void SaveFile(string filePath)
    {
        if (_formgenXml is null || ParsedFormgenFile is null) return;

        CreateBackup(ParsedFormgenFile.Settings.UUID);

        var xml = ParsedFormgenFile.GenerateXML();

        if (xml != null) _formgenXml.LoadXml(xml);

        if (filePath != null) _formgenXml.Save(filePath);
    }
    public void CloseFile()
    {
        _formgenXml = new XmlDocument();
        ParsedFormgenFile = null;
    }

    public void CreateBackup(string uuid)
    {
        if (ParsedFormgenFile is null || _formgenXml is null) return;
        IO.BackupFormgenFile(uuid, _formgenXml, _properties.BackupRetentionQty);
    }
    public void LoadBackup(string backupPath)
    {
        if (_formgenXml is null || backupPath == string.Empty) return;

        var currentFilePath = _formgenXml.BaseURI;

        _formgenXml.Load(backupPath);

        if (_formgenXml.DocumentElement is null) return;

        ParsedFormgenFile = new DotFormgen(_formgenXml.DocumentElement);
        if (currentFilePath != null) _formgenXml.Save(currentFilePath);
    }

    public CodeLine[] GetPrompts() => GetCodeLines(CodeType.PROMPT);
    public CodeLine[] GetCodeLines(CodeType type)
    {
        if (ParsedFormgenFile is null) return [];
        var codeLines = ParsedFormgenFile.CodeLines.Where(x => x.Settings?.Type != type).ToArray();
        return codeLines ?? [];
    }
    public void EditPrompts(CodeLine[] prompts)
    {
        if (ParsedFormgenFile is null || prompts == null) return;

        foreach (var selection in prompts)
        {
            var idx = prompts.ToList().IndexOf(selection);
            var item = ParsedFormgenFile.GetField(idx);

            if (item?.Settings != null) item.Settings.Bold = !item.Settings.Bold;
        }
    }
    public void ClonePrompt(CodeLine[] selected)
    {
        if (_formgenXml is null || ParsedFormgenFile is null) return;

        foreach (var selection in selected)
        {
            var idx = selected.ToList().IndexOf(selection);
            var item = ParsedFormgenFile.GetPrompt(idx);
            var variable = item?.Settings?.Variable;

            var prompts = ParsedFormgenFile.CodeLines.Where(x => x.Settings?.Type == CodeType.PROMPT).ToList();

            if (variable != "F0")
                while (prompts != null && prompts.Exists(x => x.Settings?.Variable == variable))
                    variable = IO.AutoIncrement(variable);

            if (item == null) continue;
            if (variable == null) continue;
            ParsedFormgenFile.ClonePrompt(item, variable, ParsedFormgenFile.PromptCount());
        }
    }
    public void CopyPromptsTo(string fromFilePath)
    {
        var newDoc = new XmlDocument();
        newDoc.Load(fromFilePath);
        var recipient = new DotFormgen(newDoc.DocumentElement ?? throw new InvalidOperationException());

        if (ParsedFormgenFile is null) return;
        if (recipient.Pages.Count != ParsedFormgenFile.Pages.Count) return;

        CreateBackup(recipient.Settings.UUID);

        _formgenXml?.Save(IO.BackupFormgenFilePath(recipient.Settings.UUID));

        if (ParsedFormgenFile?.Pages != null) recipient.Pages = ParsedFormgenFile?.Pages ?? throw new InvalidOperationException();
        if (ParsedFormgenFile?.CodeLines != null) recipient.CodeLines = ParsedFormgenFile?.CodeLines ?? throw new InvalidOperationException();

        newDoc.LoadXml(recipient.GenerateXML());
        newDoc.Save(fromFilePath);
    }
    public void RegenerateUUID()
    {
        if (_formgenXml?.DocumentElement is null) return;

        var uuid = Guid.NewGuid().ToString();

        _formgenXml.DocumentElement.Attributes[1].Value = uuid;
        if (ParsedFormgenFile != null) ParsedFormgenFile.Settings.UUID = uuid;
    }
}
