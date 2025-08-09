using AMFormsCST.Desktop.Converters;
using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public static class BasicStats
{
    public static UIElement GetSettingsAndPropertiesUIElements(IFormgenFileProperties properties)
    {
        var stackPanel = new StackPanel();
        var mainProperties = new List<PropertyInfo>();

        // First, separate the properties into main properties and complex types that need their own cards.
        foreach (var property in properties.GetType().GetProperties())
        {
            if (property.Name == "Settings" && property.GetValue(properties) is IFormgenFileSettings settings)
            {
                var card = CreateCardForObject(settings, "Settings");
                stackPanel.Children.Add(card);
            }
            else if (property.Name == "PromptData" && property.GetValue(properties) is PromptDataProperties promptDataProperties)
            {
                var card = CreateCardForObject(promptDataProperties, "Prompt Data");
                stackPanel.Children.Add(card);
            }
            else if (property.Name != "Settings" && property.Name != "PromptData" && property.Name != "Item") // Exclude properties handled elsewhere
            {
                mainProperties.Add(property);
            }
        }

        // If there are any main properties, create a card for them.
        if (mainProperties.Count > 0)
        {
            var propertiesCard = CreateCardForObject(properties, "Properties", mainProperties);
            // Insert the main properties card at the top.
            stackPanel.Children.Insert(0, propertiesCard);
        }

        return stackPanel;
    }

    private static CardControl CreateCardForObject(object obj, string header, IEnumerable<PropertyInfo>? propsToRender = null)
    {
        var card = new CardControl { Header = header, Margin = new Thickness(0, 0, 0, 12) };
        var grid = new Grid
        {
            Margin = new Thickness(5),
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto }, // Label
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } // Editor
            }
        };

        int rowIndex = 0;
        // Use the provided list of properties, or get all properties if none was provided.
        var properties = propsToRender ?? obj.GetType().GetProperties();

        foreach (var prop in properties)
        {
            // Skip properties that are handled by the calling method or should not be displayed.
            if (prop.Name == "Settings" || prop.Name == "PromptData")
                continue;

            // Special handling for the 'Choices' property
            if (prop.Name == "Choices" && prop.GetValue(obj) is List<string> choices && choices.Count == 0)
            {
                continue; // Don't show the editor if there are no choices.
            }

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var label = new Wpf.Ui.Controls.TextBlock
            {
                Text = prop.Name,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 8) // Add bottom margin for spacing
            };
            Grid.SetRow(label, rowIndex);
            Grid.SetColumn(label, 0);
            grid.Children.Add(label);

            var editor = CreateEditorForProperty(prop, obj);
            editor.Margin = new Thickness(0, 0, 0, 8); // Add bottom margin for spacing
            Grid.SetRow(editor, rowIndex);
            Grid.SetColumn(editor, 1);
            grid.Children.Add(editor);

            rowIndex++;
        }

        card.Content = grid;
        return card;
    }

    private static FrameworkElement CreateEditorForProperty(PropertyInfo prop, object source)
    {
        // For read-only properties or the 'ID' property, display as text.
        if (!prop.CanWrite || prop.Name == "ID")
        {
            return new Wpf.Ui.Controls.TextBlock
            {
                Text = prop.GetValue(source)?.ToString() ?? "null",
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        var binding = new Binding(prop.Name)
        {
            Source = source,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        if (prop.PropertyType == typeof(string))
        {
            var textBox = new Wpf.Ui.Controls.TextBox();
            textBox.SetBinding(Wpf.Ui.Controls.TextBox.TextProperty, binding);
            return textBox;
        }

        if (prop.PropertyType == typeof(int))
        {
            var numberBox = new NumberBox();
            numberBox.SetBinding(NumberBox.ValueProperty, binding);
            return numberBox;
        }

        if (prop.PropertyType == typeof(bool))
        {
            var toggleSwitch = new ToggleSwitch();
            toggleSwitch.SetBinding(ToggleSwitch.IsCheckedProperty, binding);
            return toggleSwitch;
        }

        if (prop.PropertyType.IsEnum)
        {
            var comboBox = new ComboBox
            {
                ItemsSource = Enum.GetValues(prop.PropertyType)
            };
            comboBox.SetBinding(ComboBox.SelectedItemProperty, binding);
            return comboBox;
        }

        if (prop.PropertyType == typeof(List<string>))
        {
            var textBox = new Wpf.Ui.Controls.TextBox
            {
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                MinHeight = 80 // Give it some default height for better usability
            };

            // Use the custom converter for the binding
            binding.Converter = new StringListToStringConverter();
            textBox.SetBinding(Wpf.Ui.Controls.TextBox.TextProperty, binding);
            return textBox;
        }

        // Fallback for unhandled types (Point, Rectangle, etc.)
        return new Wpf.Ui.Controls.TextBlock
        {
            Text = prop.GetValue(source)?.ToString() ?? "null",
            VerticalAlignment = VerticalAlignment.Center
        };
    }
}