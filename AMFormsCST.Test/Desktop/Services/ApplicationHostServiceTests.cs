using AMFormsCST.Desktop.Services;
using AMFormsCST.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using AMFormsCST.Desktop.Interfaces;

namespace AMFormsCST.Test.Desktop.Services;

public class ApplicationHostServiceTests
{
    [Fact]
    public async Task StopAsync_CallsSupportToolSaveAllSettings()
    {
        // Arrange
        var supportToolMock = new Mock<ISupportTool>();
        supportToolMock.Setup(s => s.SaveAllSettings()).Verifiable();

        var serviceProviderMock = new ServiceCollection().BuildServiceProvider();
        var hostService = new ApplicationHostService(serviceProviderMock, Mock.Of<IUpdateManagerService>(), supportToolMock.Object);

        // Act
        await hostService.StopAsync(CancellationToken.None);

        // Assert
        supportToolMock.Verify(s => s.SaveAllSettings(), Times.Once);
    }
}