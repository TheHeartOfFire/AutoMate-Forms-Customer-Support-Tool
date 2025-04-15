using AMFormsCST.Desktop.Models.Notebook;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class DashboardViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<Note> _notes = [];

    [ObservableProperty]
    private Note? _selectedNote;

    [ObservableProperty]
    private ObservableCollection<Note> _dealers = [];

    [ObservableProperty]
    private Note? _selectedDealer;

    [ObservableProperty]
    private ObservableCollection<Note> _companies = [];

    [ObservableProperty]
    private Note? _selectedCompany;

    [ObservableProperty]
    private ObservableCollection<Note> _forms = [];

    [ObservableProperty]
    private Note? _selectedForm;

    [ObservableProperty]
    private string _companyName = string.Empty;

    [ObservableProperty]
    private string _serverId = string.Empty;

    [ObservableProperty]
    private string _companyNumber = string.Empty;

    [ObservableProperty]
    private string _contactName = string.Empty;

    [ObservableProperty]
    private string _contactEmail = string.Empty;

    [ObservableProperty]
    private string _contactPhone = string.Empty;

    [ObservableProperty]
    private string _contactPhoneDelimiter = string.Empty;

    [ObservableProperty]
    private string _contactPhoneExtension = string.Empty;

    [ObservableProperty]
    private string _formName = string.Empty;

    [ObservableProperty]
    private string _formTestDeal = string.Empty;

    [ObservableProperty]
    private string _formNotes = string.Empty;

    [ObservableProperty]
    private string _caseNumber = string.Empty;

    [ObservableProperty]
    private string _generalNotes = string.Empty;
}