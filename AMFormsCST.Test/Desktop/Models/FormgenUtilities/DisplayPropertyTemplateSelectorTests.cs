using AMFormsCST.Desktop.Models.FormgenUtilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Xunit;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;

public class DisplayPropertyTemplateSelectorTests
{
    private readonly DataTemplate _stringTemplate = new DataTemplate();
    private readonly DataTemplate _intTemplate = new DataTemplate();
    private readonly DataTemplate _boolTemplate = new DataTemplate();
    private readonly DataTemplate _enumTemplate = new DataTemplate();
    private readonly DataTemplate _listStringTemplate = new DataTemplate();
    private readonly DataTemplate _readOnlyTemplate = new DataTemplate();

    private DisplayPropertyTemplateSelector CreateSelector()
    {
        return new DisplayPropertyTemplateSelector
        {
            StringTemplate = _stringTemplate,
            IntTemplate = _intTemplate,
            BoolTemplate = _boolTemplate,
            EnumTemplate = _enumTemplate,
            ListStringTemplate = _listStringTemplate,
            ReadOnlyTemplate = _readOnlyTemplate
        };
    }

    private class DummyEnum { public enum TestEnum { A, B } }

    private DisplayProperty CreateDisplayProperty(Type type, bool isReadOnly = false)
        => new DisplayProperty("Test", null, type, isReadOnly);

    [Fact]
    public void SelectTemplate_ReturnsStringTemplate_ForStringType()
    {
        var selector = CreateSelector();
        var prop = CreateDisplayProperty(typeof(string));
        var template = selector.SelectTemplate(prop, new DependencyObject());
        Assert.Same(_stringTemplate, template);
    }

    [Fact]
    public void SelectTemplate_ReturnsIntTemplate_ForIntType()
    {
        var selector = CreateSelector();
        var prop = CreateDisplayProperty(typeof(int));
        var template = selector.SelectTemplate(prop, new DependencyObject());
        Assert.Same(_intTemplate, template);
    }

    [Fact]
    public void SelectTemplate_ReturnsBoolTemplate_ForBoolType()
    {
        var selector = CreateSelector();
        var prop = CreateDisplayProperty(typeof(bool));
        var template = selector.SelectTemplate(prop, new DependencyObject());
        Assert.Same(_boolTemplate, template);
    }

    [Fact]
    public void SelectTemplate_ReturnsEnumTemplate_ForEnumType()
    {
        var selector = CreateSelector();
        var prop = CreateDisplayProperty(typeof(DummyEnum.TestEnum));
        var template = selector.SelectTemplate(prop, new DependencyObject());
        Assert.Same(_enumTemplate, template);
    }

    [Fact]
    public void SelectTemplate_ReturnsListStringTemplate_ForListStringType()
    {
        var selector = CreateSelector();
        var prop = CreateDisplayProperty(typeof(List<string>));
        var template = selector.SelectTemplate(prop, new DependencyObject());
        Assert.Same(_listStringTemplate, template);
    }

    [Fact]
    public void SelectTemplate_ReturnsReadOnlyTemplate_WhenIsReadOnly()
    {
        var selector = CreateSelector();
        var prop = CreateDisplayProperty(typeof(string), isReadOnly: true);
        var template = selector.SelectTemplate(prop, new DependencyObject());
        Assert.Same(_readOnlyTemplate, template);
    }

    [Fact]
    public void SelectTemplate_ReturnsReadOnlyTemplate_ForUnknownType()
    {
        var selector = CreateSelector();
        var prop = CreateDisplayProperty(typeof(DateTime));
        var template = selector.SelectTemplate(prop, new DependencyObject());
        Assert.Same(_readOnlyTemplate, template);
    }

    [Fact]
    public void SelectTemplate_ReturnsBase_WhenItemIsNotDisplayProperty()
    {
        var selector = CreateSelector();
        var mockContainer = new DependencyObject();
        var template = selector.SelectTemplate("not a DisplayProperty", mockContainer);
        Assert.Null(template);
    }
}