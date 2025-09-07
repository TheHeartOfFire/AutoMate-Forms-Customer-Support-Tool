using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.UserSettings;
using AMFormsCST.Desktop.Models.UserSettings;
using AMFormsCST.Desktop.ViewModels;
using Moq;
using Wpf.Ui.Appearance;
using Assert = Xunit.Assert;

namespace AMFormsCST.Test.Desktop.ViewModels.Windows;

public class MainWindowViewModelTests
{
    private readonly Mock<ISupportTool> _mockSupportTool;
    private readonly Mock<ISettings> _mockSettings;
    private readonly Mock<IUiSettings> _mockUiSettings;
    private readonly Mock<ILogService> _mockLogService;
    private readonly ThemeSetting _themeSetting;
    private readonly AlwaysOnTopSetting _aotSetting;

    public MainWindowViewModelTests()
    {
        // 1. Mock the dependency graph needed by the MainWindowViewModel.
        _mockSupportTool = new Mock<ISupportTool>();
        _mockSettings = new Mock<ISettings>();
        _mockUiSettings = new Mock<IUiSettings>();
        _mockLogService = new Mock<ILogService>();

        // 2. Create concrete setting objects to be returned by the mocks.
        _themeSetting = new ThemeSetting { Theme = ApplicationTheme.Dark };
        _aotSetting = new AlwaysOnTopSetting { IsEnabled = true };

        // 3. Configure the mocks.
        _mockUiSettings.Setup(s => s.Settings).Returns([_themeSetting, _aotSetting]);
        _mockSettings.Setup(s => s.UiSettings).Returns(_mockUiSettings.Object);
        _mockSupportTool.Setup(st => st.Settings).Returns(_mockSettings.Object);
    }

    [Fact]
    public void Constructor_InitializesProperties_FromSettings()
    {
        // Arrange & Act
        var viewModel = new MainWindowViewModel(_mockSupportTool.Object, _mockLogService.Object);

        // Assert
        Assert.True(viewModel.IsWindowTopmost);
        Assert.False(viewModel.IsLightTheme); // Dark theme means IsLightTheme is false
    }

    [Fact]
    public void IsWindowTopmost_WhenChanged_UpdatesAndSavesSetting()
    {
        // Arrange
        var viewModel = new MainWindowViewModel(_mockSupportTool.Object, _mockLogService.Object);
        Assert.True(viewModel.IsWindowTopmost); // Initial state check

        // Act
        viewModel.IsWindowTopmost = false;

        // Assert
        Assert.False(_aotSetting.IsEnabled); // Verify the underlying setting was changed
        _mockSupportTool.Verify(st => st.SaveAllSettings(), Times.Once);
    }

    [Fact]
    public void IsLightTheme_WhenChanged_UpdatesThemeAndSavesSetting()
    {
        // Arrange
        var viewModel = new MainWindowViewModel(_mockSupportTool.Object, _mockLogService.Object);
        Assert.False(viewModel.IsLightTheme); // Initial state check

        // Act
        viewModel.IsLightTheme = true;

        // Assert
        Assert.Equal(ApplicationTheme.Light, _themeSetting.Theme); // Verify the theme was changed
        _mockSupportTool.Verify(st => st.SaveAllSettings(), Times.Once);
    }
}