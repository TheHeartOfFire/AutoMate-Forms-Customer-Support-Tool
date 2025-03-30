using AMFormsCST.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMFormsCST.Desktop.Controls.FormgenUtilities;
public class FormProperties : System.Windows.Controls.Control, IFormgenFileProperties
{

    /// <summary>Identifies the <see cref="ItemsSource"/> dependency property.</summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(object),
        typeof(FormProperties),
        new PropertyMetadata(null)
    );
    /// <summary>Identifies the <see cref="Version"/> dependency property.</summary>
    public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
        nameof(FormProperties.Version),
        typeof(string),
        typeof(FormProperties),
        new PropertyMetadata(string.Empty)
    );
    /// <summary>Identifies the <see cref="PublishedUUID"/> dependency property.</summary>
    public static readonly DependencyProperty PublishedUUIDProperty = DependencyProperty.Register(
        nameof(PublishedUUID),
        typeof(string),
        typeof(FormProperties),
        new PropertyMetadata(string.Empty)
    );
    /// <summary>Identifies the <see cref="LegacyImport"/> dependency property.</summary>
    public static readonly DependencyProperty LegacyImportProperty = DependencyProperty.Register(
        nameof(LegacyImport),
        typeof(bool),
        typeof(FormProperties),
        new PropertyMetadata(false)
    );
    /// <summary>Identifies the <see cref="TotalPages"/> dependency property.</summary>
    public static readonly DependencyProperty TotalPagesProperty = DependencyProperty.Register(
        nameof(TotalPages),
        typeof(int),
        typeof(FormProperties),
        new PropertyMetadata(0)
    );
    /// <summary>Identifies the <see cref="DefaultPoints"/> dependency property.</summary>
    public static readonly DependencyProperty DefaultPointsProperty = DependencyProperty.Register(
        nameof(DefaultPoints),
        typeof(int),
        typeof(FormProperties),
        new PropertyMetadata(0)
    );
    /// <summary>Identifies the <see cref="MissingSourceJpeg"/> dependency property.</summary>
    public static readonly DependencyProperty MissingSourceJpegProperty = DependencyProperty.Register(
        nameof(MissingSourceJpeg),
        typeof(bool),
        typeof(FormProperties),
        new PropertyMetadata(false)
    );
    /// <summary>Identifies the <see cref="Duplex"/> dependency property.</summary>
    public static readonly DependencyProperty DuplexProperty = DependencyProperty.Register(
        nameof(Duplex),
        typeof(bool),
        typeof(FormProperties),
        new PropertyMetadata(false)
    );
    /// <summary>Identifies the <see cref="MaxAccessoryLines"/> dependency property.</summary>
    public static readonly DependencyProperty MaxAccessoryLinesProperty = DependencyProperty.Register(
        nameof(MaxAccessoryLines),
        typeof(int),
        typeof(FormProperties),
        new PropertyMetadata(0)
    );
    /// <summary>Identifies the <see cref="PrePrintedLaserForm"/> dependency property.</summary>
    public static readonly DependencyProperty PrePrintedLaserFormProperty = DependencyProperty.Register(
        nameof(PrePrintedLaserForm),
        typeof(bool),
        typeof(FormProperties),
        new PropertyMetadata(false)
        );

    public object? ItemsSource { get => GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }
    public string? Version { get => (string)GetValue(VersionProperty); set => SetValue(VersionProperty, value); }
    public string? PublishedUUID { get => (string)GetValue(PublishedUUIDProperty); set => SetValue(PublishedUUIDProperty, value); }
    public bool LegacyImport { get => (bool)GetValue(LegacyImportProperty); set => SetValue(LegacyImportProperty, value); }
    public int TotalPages { get => (int)GetValue(TotalPagesProperty); set => SetValue(TotalPagesProperty, value); }
    public int DefaultPoints { get => (int)GetValue(DefaultPointsProperty); set => SetValue(DefaultPointsProperty, value); }
    public bool MissingSourceJpeg { get => (bool)GetValue(MissingSourceJpegProperty); set => SetValue(MissingSourceJpegProperty, value); }
    public bool Duplex { get => (bool)GetValue(DuplexProperty); set => SetValue(DuplexProperty, value); }
    public int MaxAccessoryLines { get => (int)GetValue(MaxAccessoryLinesProperty); set => SetValue(MaxAccessoryLinesProperty, value); }
    public bool PrePrintedLaserForm { get => (bool)GetValue(PrePrintedLaserFormProperty); set => SetValue(PrePrintedLaserFormProperty, value); }

}
