using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class DashboardViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<Note> _notes = [];

    [ObservableProperty]
    private Note? _selectedNote; 
    
    [ObservableProperty]
    private bool _isDebugMode = true;

    [ObservableProperty]
    private Visibility _debugVisibility = Visibility.Collapsed;

    public DashboardViewModel()
    {
        // Initialize the Notes collection with a default Note if needed
        if (Notes.Count == 0)
        {
            Notes.Add(new Note());
            Notes.Add(new Note());
            Notes.Add(new Note());
        }
        // Set the initial selected note to the first one in the collection
        SelectedNote = Notes[0];
        Notes[0].IsSelected = true;
        if (IsDebugMode) _debugVisibility = Visibility.Visible;
    }

    [RelayCommand]
    private void OnNoteClicked(Guid caseId)
    {
        if (caseId == SelectedNote?.Id) return;

        foreach (var note in Notes)
        {
            if (note.Id == caseId)
            {
                SelectedNote = note;
                note.IsSelected = true;
                continue;
            }
            note.IsSelected = false;
        }
    } 
    
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
}