using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace AMFormsCST.Desktop.ViewModels.Pages.Tools;

public partial class FormgenUtilitiesViewModel : ViewModel 
{
    private static string _rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string _formgenPath = System.IO.Path.Combine(_rootPath, "AMFormsCST", "Formgen");
    private static string _backupPath = System.IO.Path.Combine(_rootPath, "AMFormsCST", "Formgen", "Backup");
    private static string _formgenFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "formgen");
    [ObservableProperty]
    private IFormgenUtils _formgenUtils = SupportTool.SupportToolInstance.FormgenUtils;

    [ObservableProperty]
    private string _uuid = string.Empty;

    [ObservableProperty]
    private bool _isDebugMode = false;

    [ObservableProperty]
    private Visibility _debugVisibility = Visibility.Collapsed;

    public static OpenFileDialog OpenWindow { get; set; } =
        new OpenFileDialog
        {
            InitialDirectory = _formgenFolderPath,
            Filter = "Formgen Files (*.formgen)|*.formgen"
        };
    public static OpenFolderDialog SaveWindow { get; set; } =
        new OpenFolderDialog
        {
            InitialDirectory = _formgenFolderPath
        };

    /// <summary>
    /// Set the state of Debug Mode
    /// if <param name="debugModeEnable"></param> is null, it will toggle the current state
    /// </summary>
    /// <param name="debugModeEnable"></param>
    public void ToggleDebugMode(bool? debugModeEnable)
    {
        if (!debugModeEnable.HasValue)
            debugModeEnable = !IsDebugMode;

        IsDebugMode = debugModeEnable.Value;
        DebugVisibility = IsDebugMode ? Visibility.Visible : Visibility.Collapsed;
    }

    public void RegenerateUUID()
    {
        if (FormgenUtils is null || FormgenUtils.ParsedFormgenFile is null) return;

        FormgenUtils.RegenerateUUID();
        Uuid = FormgenUtils.ParsedFormgenFile.Settings.UUID;
    }
    public void SaveFormgenFile(string path, string? newFileName = null, bool renameImage = false)
    {
        if (FormgenUtils is null || FormgenUtils.ParsedFormgenFile is null) return;

        if (newFileName is not null)
        {
            FormgenUtils.RenameFile(
                newFileName,
                renameImage);
        }

    }
    public static MessageBoxResult Confirm(string message, string caption) => 
        MessageBox.Show(
                 message,
                 caption,
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Warning);

    public void SaveBackup()
    {
        FormgenUtils.CreateBackup();
    }
    
}