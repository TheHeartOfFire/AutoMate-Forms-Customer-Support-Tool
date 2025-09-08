namespace AMFormsCST.Core.Interfaces.UserSettings;
public interface IUiSettings : ISetting
{
    List<ISetting> Settings { get; set; }
}
