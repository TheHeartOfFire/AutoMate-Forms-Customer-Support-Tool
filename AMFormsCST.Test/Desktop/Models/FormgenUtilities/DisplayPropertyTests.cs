using AMFormsCST.Desktop.Models.FormgenUtilities;
using System.Reflection;
using Xunit;

namespace AMFormsCST.Test.Desktop.Models.FormgenUtilities;

public class DisplayPropertyTests
{
    private class DummySource
    {
        public string StringProp { get; set; } = "Initial";
        public int IntProp { get; set; } = 42;
        public bool BoolProp { get; set; } = true;
    }

    [Fact]
    public void Constructor_SetsNameTypeValue_ForSummaryProperty()
    {
        // Arrange
        var name = "PropertyName";
        var value = "PropertyValue";
        var type = typeof(string);

        // Act
        var prop = new DisplayProperty(name, value, type);

        // Assert
        Assert.Equal(name, prop.Name);
        Assert.Equal(type, prop.Type);
        Assert.Equal(value, prop.Value);
        Assert.True(prop.IsReadOnly);
    }

    [Fact]
    public void Constructor_SetsNameTypeValue_ForEditableProperty()
    {
        // Arrange
        var source = new DummySource();
        var propInfo = typeof(DummySource).GetProperty(nameof(DummySource.StringProp))!;

        // Act
        var prop = new DisplayProperty(source, propInfo);

        // Assert
        Assert.Equal(propInfo.Name, prop.Name);
        Assert.Equal(propInfo.PropertyType, prop.Type);
        Assert.Equal(source.StringProp, prop.Value);
        Assert.False(prop.IsReadOnly);
    }

    [Fact]
    public void Value_Setter_UpdatesSourceProperty_WhenEditable()
    {
        // Arrange
        var source = new DummySource();
        var propInfo = typeof(DummySource).GetProperty(nameof(DummySource.StringProp))!;
        var prop = new DisplayProperty(source, propInfo);

        // Act
        prop.Value = "Updated";

        // Assert
        Assert.Equal("Updated", prop.Value);
        Assert.Equal("Updated", source.StringProp);
    }

    [Fact]
    public void Value_Setter_DoesNotUpdateSource_WhenReadOnly()
    {
        // Arrange
        var name = "ReadOnlyProp";
        var value = "ReadOnlyValue";
        var type = typeof(string);
        var prop = new DisplayProperty(name, value, type, isReadOnly: true);

        // Act
        prop.Value = "NewValue";

        // Assert
        Assert.Equal("NewValue", prop.Value);
        // No source to update, just check value is set
    }

    [Fact]
    public void Value_Setter_InvokesPropertyChanged()
    {
        // Arrange
        var source = new DummySource();
        var propInfo = typeof(DummySource).GetProperty(nameof(DummySource.IntProp))!;
        var prop = new DisplayProperty(source, propInfo);
        bool eventRaised = false;
        prop.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(DisplayProperty.Value))
                eventRaised = true;
        };

        // Act
        prop.Value = 100;

        // Assert
        Assert.True(eventRaised);
        Assert.Equal(100, prop.Value);
        Assert.Equal(100, source.IntProp);
    }

    [Fact]
    public void Value_Setter_UsesSetMethodIfAvailable()
    {
        // Arrange
        var source = new DummySource();
        var propInfo = typeof(DummySource).GetProperty(nameof(DummySource.BoolProp))!;
        var prop = new DisplayProperty(source, propInfo);

        // Act
        prop.Value = false;

        // Assert
        Assert.Equal(false, prop.Value);
        Assert.False(source.BoolProp);
    }
}