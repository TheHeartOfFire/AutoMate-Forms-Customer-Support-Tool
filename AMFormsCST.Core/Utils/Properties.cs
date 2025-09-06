using AMFormsCST.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Utils;
public class Properties
{
    private readonly ILogService? _logger;

    public FormgenUtilsProperties FormgenUtils { get; set; }
    public ErrorMessages ErrorMessages { get; set; }

    [JsonConstructor]
    public Properties(FormgenUtilsProperties formgenUtils, ErrorMessages errorMessages)
    {
        FormgenUtils = formgenUtils;
        ErrorMessages = errorMessages;
    }

    // Logger-initializing constructor for runtime use only
    public Properties(ILogService? logger = null)
    {
        _logger = logger;
        FormgenUtils = new FormgenUtilsProperties(_logger);
        ErrorMessages = new ErrorMessages(_logger);
        _logger?.LogInfo("Properties initialized.");
    }
}

public class FormgenUtilsProperties
{
    private readonly ILogService? _logger;
    private uint _backupRetentionQty;

    public uint BackupRetentionQty
    {
        get => _backupRetentionQty;
        set
        {
            if (_backupRetentionQty != value)
            {
                _backupRetentionQty = value;
                _logger?.LogInfo($"BackupRetentionQty set to {value}");
            }
        }
    }

    [JsonConstructor]
    public FormgenUtilsProperties(uint backupRetentionQty)
    {
        _backupRetentionQty = backupRetentionQty;
    }

    // Runtime constructor for logger injection
    public FormgenUtilsProperties(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("FormgenUtilsProperties initialized.");
    }
}

public class ErrorMessages
{
    private readonly ILogService? _logger;
    public NotesErrorMessages Notes { get; set; }
    [JsonConstructor]
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

    private string _noNoteInListMessage = "There is no {0} in {1} that matches the {0}(s) provided.";
    private string _noteMissingMessage = "{0} is missing:\n{1}";
    private string _bothNotesMissingMessage = "Both notes are missing:\n{0}\n{1}";
    private string _emptyListMessage = "There are no {0}s in {1} to load.";

    public string NoNoteInListMessage
    {
        get => _noNoteInListMessage;
        set
        {
            if (_noNoteInListMessage != value)
            {
                _noNoteInListMessage = value;
                _logger?.LogInfo($"NoNoteInListMessage set to {value}");
            }
        }
    }
    public string NoteMissingMessage
    {
        get => _noteMissingMessage;
        set
        {
            if (_noteMissingMessage != value)
            {
                _noteMissingMessage = value;
                _logger?.LogInfo($"NoteMissingMessage set to {value}");
            }
        }
    }
    public string BothNotesMissingMessage
    {
        get => _bothNotesMissingMessage;
        set
        {
            if (_bothNotesMissingMessage != value)
            {
                _bothNotesMissingMessage = value;
                _logger?.LogInfo($"BothNotesMissingMessage set to {value}");
            }
        }
    }
    public string EmptyListMessage
    {
        get => _emptyListMessage;
        set
        {
            if (_emptyListMessage != value)
            {
                _emptyListMessage = value;
                _logger?.LogInfo($"EmptyListMessage set to {value}");
            }
        }
    }
    [JsonConstructor]   
    public NotesErrorMessages()
    {
        
    }
    public NotesErrorMessages(ILogService? logger = null)
    {
        _logger = logger;
        _logger?.LogInfo("NotesErrorMessages initialized.");
    }
}
