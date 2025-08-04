namespace AMFormsCST.Core.Interfaces.UserSettings;
public interface ISettings
{
    List<ISetting> AllSettings { get; }
    IUserSettings UserSettings { get; set; }
    List<ISetting> UiSettings { get; set; }
}