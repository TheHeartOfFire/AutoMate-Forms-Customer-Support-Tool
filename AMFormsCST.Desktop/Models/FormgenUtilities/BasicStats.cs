using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using static AMFormsCST.Core.Types.FormgenUtils.FormgenFileStructure.PromptDataSettings;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class BasicStats : IFormgenFileProperties
{
    public int Total { get; set; }
    public IFormgenFileSettings Settings { get; set; } = new PageSettings();

    public StackPanel GetUIElements() => new()
    {
        Children = {
            
            new Label { Content = $"Total: {Total}" },
    }
    };

    public static StackPanel GetSettingsAndPropertiesUIElements(IFormgenFileProperties properties)
    {
        var propertiesPanel = new StackPanel
        {
            Margin = new Thickness(5)
        };

        propertiesPanel.Children.Add(new Label
        {
            Content = "Properties",
            FontWeight = FontWeights.SemiBold,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(5, 10, 0, 0),
            FontSize = 28
        });

        foreach (var property in properties.GetType().GetProperties())
        {
            if (!property.Name.Equals("Settings", StringComparison.OrdinalIgnoreCase) &&
                !property.Name.Equals("PromptData", StringComparison.OrdinalIgnoreCase))
                propertiesPanel.Children.Add(ParseProperty(properties, property));
        }
        var settingsPanel = new StackPanel
        {
            Margin = new Thickness(5)
        };

        settingsPanel.Children.Add(new Label
        {
            Content = "Settings",
            FontWeight = FontWeights.SemiBold,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(5, 10, 0, 0),
            FontSize = 28
        });

        foreach (var property in properties.Settings.GetType().GetProperties())
        {
            settingsPanel.Children.Add(ParseProperty(properties.Settings, property));
        }

        var output = new StackPanel
        {
            Children =
            {
                propertiesPanel,
                settingsPanel

            },
            Orientation = Orientation.Horizontal
        };

        if (properties is CodeLineProperties codeLineProps)
        {
            output.Children.Add(new StackPanel
            {
                Children =
                {
                    new Label
                    {
                        Content = "Prompt Data",
                        HorizontalAlignment = HorizontalAlignment.Center
                    },
                    codeLineProps.PromptData.GetUIElements()
                }
            });

        } 

        return output;
    }

    internal static StackPanel ParseProperty(object properties, PropertyInfo propInfo)
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(5)
        };

        if (propInfo.GetValue(properties) is List<string> list)
        {
            panel.Orientation = Orientation.Vertical;
            panel.Children.Add(new Label()
            {
                Content = propInfo.Name + ":",
                VerticalAlignment = VerticalAlignment.Center
            });
            foreach (var item in list)
            {
                panel.Children.Add(new Label()
                {
                    Content = "\t-" + item
                });
            }
            return panel;
        }

        panel.Children.Add(new Label()
        {
            Content = propInfo.Name,
            VerticalAlignment = VerticalAlignment.Center
        });

        var boolButton = ParseBool(properties, propInfo);
        if (boolButton is not null)
        {
            panel.Children.Add(boolButton);
            return panel;
        }

        panel.Children.Add(ParseString(properties, propInfo));
        return panel;
    }

    private static Wpf.Ui.Controls.TextBox ParseString(object properties, PropertyInfo propInfo)
    {
        bool isUUID = propInfo.Name.Equals("PublishedUUID", StringComparison.OrdinalIgnoreCase);
        var box = new Wpf.Ui.Controls.TextBox
        {
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 0, 0, 0),
            Background = Brushes.Transparent,
            BorderBrush = Brushes.Transparent,
            IsReadOnly = isUUID,
            ToolTip = isUUID
                ? "This is a UUID and cannot be changed here. It will only update if you regenerate the UUID above."
                : $"Enter a value for {propInfo.Name}",
        };

        box.SetBinding(Wpf.Ui.Controls.TextBox.TextProperty, new System.Windows.Data.Binding(propInfo.Name)
        {
            Source = properties,
            Mode = System.Windows.Data.BindingMode.TwoWay
        });
        return box;
    }

    private static ToggleButton? ParseBool(object properties, PropertyInfo property)
    {
        if (bool.TryParse(properties.GetType().GetProperty(property.Name)?.GetValue(properties)?.ToString(), out bool boolValue))
        {
            var button = new ToggleButton
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0),
                IsChecked = boolValue,
                Height = 25,
                Width = 25

            };
            button.SetBinding(ToggleButton.IsCheckedProperty, new System.Windows.Data.Binding(property.Name)
            {
                Source = properties,
                Mode = System.Windows.Data.BindingMode.TwoWay
            });
            return button;
        }
        return null;
    }
}