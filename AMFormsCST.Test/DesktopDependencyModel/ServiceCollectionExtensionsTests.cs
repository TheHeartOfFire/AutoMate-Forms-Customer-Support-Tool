using AMFormsCST.Desktop.DependencyModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

public class ServiceCollectionExtensionsTests
{
    private class TestClassA { }
    private class TestClassB { }
    private class NotInNamespace { }
    private class ViewModel { } // Should be excluded

    [Fact]
    public void AddTransientFromNamespace_RegistersClassesFromNamespace()
    {
        // Arrange
        var services = new ServiceCollection();
        var namespaceName = typeof(TestClassA).Namespace;
        var assembly = typeof(TestClassA).Assembly;

        // Act
        services.AddTransientFromNamespace(namespaceName, assembly);

        // Assert
        Assert.Contains(services, d => d.ServiceType == typeof(TestClassA));
        Assert.Contains(services, d => d.ServiceType == typeof(TestClassB));
        Assert.DoesNotContain(services, d => d.ServiceType == typeof(NotInNamespace));
    }

    [Fact]
    public void AddTransientFromNamespace_DoesNotRegisterViewModelClass()
    {
        // Arrange
        var services = new ServiceCollection();
        var namespaceName = typeof(ViewModel).Namespace;
        var assembly = typeof(ViewModel).Assembly;

        // Act
        services.AddTransientFromNamespace(namespaceName, assembly);

        // Assert
        Assert.DoesNotContain(services, d => d.ServiceType == typeof(ViewModel));
    }

    [Fact]
    public void AddTransientFromNamespace_DoesNotRegisterDuplicateTypes()
    {
        // Arrange
        var services = new ServiceCollection();
        var namespaceName = typeof(TestClassA).Namespace;
        var assembly = typeof(TestClassA).Assembly;

        // Add TestClassA manually
        services.AddTransient<TestClassA>();

        // Act
        services.AddTransientFromNamespace(namespaceName, assembly);

        // Assert
        // Only one registration for TestClassA
        Assert.Equal(1, services.Count(d => d.ServiceType == typeof(TestClassA)));
    }
}