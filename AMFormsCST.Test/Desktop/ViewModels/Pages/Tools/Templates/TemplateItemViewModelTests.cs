using AMFormsCST.Desktop.ViewModels.Pages.Tools.Templates;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using AMFormsCST.Core.Interfaces;
using Moq;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace AMFormsCST.Test.Desktop.ViewModels.Pages.Tools.Templates;

public class TemplateItemViewModelTests
{
    private static TextTemplate CreateTextTemplate(string text = "Hello {0}!", string name = "Test", string description = "Desc")
    {
        var templateType = TextTemplate.TemplateType.Other;
        return new TextTemplate(name, description, text, templateType);
    }

    private static Mock<ISupportTool> CreateSupportToolMock()
    {
        var mock = new Mock<ISupportTool>();
        mock.SetupAllProperties();
        return mock;
    }

    [Fact]
    public void Constructor_InitializesProperties()
    {
        var template = CreateTextTemplate();
        var supportTool = CreateSupportToolMock().Object;

        var vm = new TemplateItemViewModel(template, supportTool);

        Assert.Equal(template, vm.Template);
        Assert.NotNull(vm.Variables);
        Assert.False(vm.IsSelected);
        Assert.NotEqual(Guid.Empty, vm.Id);
    }

    [Fact]
    public void Select_SetsIsSelectedTrue()
    {
        var vm = new TemplateItemViewModel(CreateTextTemplate(), CreateSupportToolMock().Object);
        vm.Select();
        Assert.True(vm.IsSelected);
    }

    [Fact]
    public void Deselect_SetsIsSelectedFalse()
    {
        var vm = new TemplateItemViewModel(CreateTextTemplate(), CreateSupportToolMock().Object);
        vm.Select();
        vm.Deselect();
        Assert.False(vm.IsSelected);
    }
}