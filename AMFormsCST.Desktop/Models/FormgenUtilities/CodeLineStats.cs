using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMFormsCST.Desktop.Models.FormgenUtilities;
public class CodeLineStats : BasicStats, IFormgenFileProperties
{
    public int Init { get; set; }
    public int Prompts { get; set; }
    public int PostPrompts { get; set; }

    public new UIElement GetUIElements()
    {
        var statsPanel = new StackPanel
        {
            Margin = new Thickness(5)
        };

        statsPanel.Children.Add(new Label
        {
            Content = "Stats",
            FontWeight = FontWeights.SemiBold,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(5, 10, 0, 0),
            FontSize = 28
        });

        statsPanel.Children.Add(new Label
        {
            Content = $"Total Code Lines: {Total}",
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 10, 0, 0)
        });
        statsPanel.Children.Add(new Label
        {
            Content = $"Init: {Init}",
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 10, 0, 0)
        });
        statsPanel.Children.Add(new Label
        {
            Content = $"Prompts: {Prompts}",
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 10, 0, 0)
        });
        statsPanel.Children.Add(new Label
        {
            Content = $"Post Prompts: {PostPrompts}",
            FontWeight = FontWeights.Bold,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 10, 0, 0)
        });

        return new Border()
        {
            Child = statsPanel,
            BorderBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(3)
        };
    }
}
