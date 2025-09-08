using AMFormsCST.Core.Interfaces;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Utils;
public class Properties
{
    private readonly ILogService? _logger;

    [JsonInclude]
    public FormgenUtilsProperties FormgenUtils { get; set; }
    [JsonInclude]
    public ErrorMessages ErrorMessages { get; set; }

    public Properties()
    {
        FormgenUtils = new FormgenUtilsProperties();
        ErrorMessages = new ErrorMessages();
    }

    public Properties(ILogService? logger = null) : this()
    {
        _logger = logger;
        // Re-initialize with logger if provided
        FormgenUtils = new FormgenUtilsProperties(logger);
        ErrorMessages = new ErrorMessages(logger);
        _logger?.LogInfo("Properties initialized.");
    }
}

public class FormgenUtilsProperties
{
    private readonly ILogService? _logger;
    
    [JsonInclude]
    public uint BackupRetentionQty { get; set; }

    public FormgenUtilsProperties() { }

    public FormgenUtilsProperties(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("FormgenUtilsProperties initialized.");
    }
}

public class ErrorMessages
{
    private readonly ILogService? _logger;
    [JsonInclude]
    public NotesErrorMessages Notes { get; set; }
    
    public ErrorMessages()
    {
        Notes = new NotesErrorMessages();
    }

    public ErrorMessages(ILogService? logger = null)
    {
        _logger = logger;
        Notes = new NotesErrorMessages(_logger);
        _logger?.LogInfo("ErrorMessages initialized.");
    }
}

public class NotesErrorMessages
{
    private readonly ILogService? _logger;

    [JsonInclude]
    public string NoNoteInListMessage { get; set; } = "There is no {0} in {1} that matches the {0}(s) provided.";
    [JsonInclude]
    public string NoteMissingMessage { get; set; } = "{0} is missing:\n{1}";
    [JsonInclude]
    public string BothNotesMissingMessage { get; set; } = "Both notes are missing:\n{0}\n{1}";
    [JsonInclude]
    public string EmptyListMessage { get; set; } = "There are no {0}s in {1} to load.";

    public NotesErrorMessages() { }

    public NotesErrorMessages(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("NotesErrorMessages initialized.");
    }
}
