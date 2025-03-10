using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Utils;
public class Properties
{
    public FormgenUtilsProperties FormgenUtils { get; set; } = new();
    public ErrorMessages ErrorMessages { get; set; } = new();

}
public class FormgenUtilsProperties
{
    public uint BackupRetentionQty { get; set; }
}

public class ErrorMessages
{
    public NotesErrorMessages Notes { get; set; } = new();
}

public class NotesErrorMessages
{
    public string NoNoteInListMessage { get; set; } = "There is no {0} in {1} that matches the {0}(s) provided.";
    public string NoteMissingMessage { get; set; } = "{0} is missing:\n{1}";
    public string BothNotesMissingMessage { get; set; } = "Both notes are missing:\n{0}\n{1}";
    public string EmptyListMessage { get; set; } = "There are no {0}s in {1} to load.";
}
