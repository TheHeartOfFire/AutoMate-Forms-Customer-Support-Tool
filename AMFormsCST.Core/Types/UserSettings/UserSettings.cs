using AMFormsCST.Core.Interfaces.UserSettings;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Types.UserSettings;

public class UserSettings : IUserSettings
{
    public IOrgVariables Organization { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ApplicationFilesPath { get; set; } = string.Empty;
    public string ExtSeparator { get; set; } = "x";

    [JsonConstructor]
    public UserSettings(IOrgVariables organization)
    {
        Organization = organization;
    }
}
