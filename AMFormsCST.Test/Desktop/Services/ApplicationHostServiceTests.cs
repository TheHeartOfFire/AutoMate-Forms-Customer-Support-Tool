using AMFormsCST.Desktop.Services;
using AMFormsCST.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xunit;
using AMFormsCST.Desktop.Interfaces;
namespace AMFormsCST.Test.Desktop.Services;
public class ApplicationHostServiceTests
{
    [Fact]
    public async Task StartAsync_CallsHandleActivationAsyncAndUpdateManager()
    {
        // Arrange
        var updateManagerMock = new Mock<IUpdateManagerService>();
        updateManagerMock.Setup(m => m.CheckForUpdatesOnStartupAsync()).Returns(Task.CompletedTask).Verifiable();

        var supportToolMock = new Mock<ISupportTool>();
        var windowMock = new Mock<IWindow>();
        windowMock.SetupAdd(w => w.Loaded += It.IsAny<RoutedEventHandler>()).Verifiable();
        windowMock.Setup(w => w.Show()).Verifiable();

        var serviceProviderMock = new ServiceCollection()
            .AddSingleton(windowMock.Object)
            .BuildServiceProvider();

        var hostService = new ApplicationHostService(serviceProviderMock, updateManagerMock.Object, supportToolMock.Object);

        // Act
        await hostService.StartAsync(CancellationToken.None);

        // Assert
        updateManagerMock.Verify(m => m.CheckForUpdatesOnStartupAsync(), Times.Once);
    }

    [Fact]
    public async Task StopAsync_CallsSupportToolSaveAllSettings()
    {
        // Arrange
        var updateManagerMock = new Mock<IUpdateManagerService>();
        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.Setup(s => s.SaveAllSettings()).Verifiable();

        var serviceProviderMock = new ServiceCollection().BuildServiceProvider();
        var hostService = new ApplicationHostService(serviceProviderMock, updateManagerMock.Object, supportToolMock.Object);

        // Act
        await hostService.StopAsync(CancellationToken.None);

        // Assert
        supportToolMock.Verify(s => s.SaveAllSettings(), Times.Once);
    }
}