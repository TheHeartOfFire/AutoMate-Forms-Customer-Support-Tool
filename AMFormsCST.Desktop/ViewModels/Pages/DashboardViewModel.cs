using AMFormsCST.Desktop.Models.Notebook;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class DashboardViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<Note> _notes = [new()];

    [ObservableProperty]
    private Note? _selectedNote;

    public const string DefaultCaseText = "Enter a Case Number...";
}