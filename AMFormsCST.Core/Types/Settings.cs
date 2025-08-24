using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AMFormsCST.Core.Types;
public class Settings : ISettings
{
    private readonly ILogService? _logger;

    [JsonIgnore] // This property is for convenience and should not be serialized
    public List<ISetting> AllSettings
    {
        get
        {
            return [UserSettings, UiSettings];
        }
    }

    [JsonInclude]
    public IUserSettings UserSettings
    {
        get => _userSettings;
        set
        {
            if (value is null)
            {
                var ex = new ArgumentNullException(nameof(value), "User settings cannot be null.");
                _logger?.LogError("Attempted to set UserSettings to null.", ex);
                throw ex;
            }
            if (_userSettings != value)
            {
                _userSettings = value;
                _logger?.LogInfo("UserSettings property set.");
            }
        }
    }

    [JsonInclude]
    public IUiSettings UiSettings
    {
        get => _uiSettings;
        set
        {
            if (value is null)
            {
                var ex = new ArgumentNullException(nameof(value), "UI settings cannot be null.");
                _logger?.LogError("Attempted to set UiSettings to null.", ex);
                throw ex;
            }
            if (_uiSettings != value)
            {
                _uiSettings = value;
                _logger?.LogInfo("UiSettings property set.");
            }
        }
    }

    private IUserSettings _userSettings;
    private IUiSettings _uiSettings;

    public Settings(IUserSettings userSettings, IUiSettings uiSettings, ILogService? logger = null)
    {
        _logger = logger;
        if (userSettings is null)
        {
            var ex = new ArgumentNullException(nameof(userSettings), "User settings cannot be null.");
            _logger?.LogError("Constructor received null for userSettings.", ex);
            throw ex;
        }
        if (uiSettings is null)
        {
            var ex = new ArgumentNullException(nameof(uiSettings), "UI settings cannot be null.");
            _logger?.LogError("Constructor received null for uiSettings.", ex);
            throw ex;
        }
        _userSettings = userSettings;
        _uiSettings = uiSettings;
        _logger?.LogInfo("Settings initialized.");
    }
}
