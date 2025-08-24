using AMFormsCST.Core;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.Interfaces;
using AMFormsCST.Desktop.Models;
using AMFormsCST.Desktop.Services;
using AMFormsCST.Desktop.Types;
using AMFormsCST.Desktop.ViewModels.Dialogs;
using AMFormsCST.Desktop.ViewModels.Pages.Tools;
using AMFormsCST.Desktop.Views.Dialogs;
using AMFormsCST.Desktop.Views.Pages.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using Wpf.Ui;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class DashboardViewModel : ViewModel
{
    private readonly ILogService _logger;

    [ObservableProperty]
    private ManagedObservableCollection<NoteModel> _notes = new(() => new NoteModel(""));

    public string DebugSelectedNoteId => SelectedNote?.Id.ToString() ?? "None";

    private NoteModel _selectedNote;
    public NoteModel SelectedNote
    {
        get => _selectedNote;
        set
        {
            if (_selectedNote != null)
            {
                _selectedNote.PropertyChanged -= OnNoteModelPropertyChanged;
            }

            SetProperty(ref _selectedNote, value);

            if (_selectedNote != null)
            {
                _selectedNote.PropertyChanged += OnNoteModelPropertyChanged;
                _logger?.LogInfo($"Selected note changed: {_selectedNote.Id}");
            }
        }
    }

    [ObservableProperty]
    private bool _isDebugMode = false;

    [ObservableProperty]
    private Visibility _debugVisibility = Visibility.Collapsed;

    private int _uiRefreshCounter;
    public int UiRefreshCounter
    {
        get => _uiRefreshCounter;
        private set => SetProperty(ref _uiRefreshCounter, value); 
    }

    private readonly ISupportTool _supportTool;
    private readonly IDialogService _dialogService;
    private readonly IFileSystem _fileSystem;

    public DashboardViewModel(ISupportTool supportTool, IDialogService dialogService, IFileSystem fileSystem, ILogService logger)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _fileSystem = fileSystem;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInfo("DashboardViewModel initialized.");

        if (_supportTool.Notebook.Notes.Count == 0)
        {
            Notes.Add(new NoteModel(_supportTool.Settings.UserSettings.ExtSeparator, _logger));
            _logger.LogInfo("Added blank note to Notes collection.");
        }

        foreach(var note in _supportTool.Notebook.Notes)
        {
            var noteModel = new NoteModel(note, _supportTool.Settings.UserSettings.ExtSeparator, _logger);
            noteModel.CoreType = note;
            noteModel.Parent = this;
            noteModel.PropertyChanged += OnNoteModelPropertyChanged;
            Notes.Add(noteModel);
        }

        _selectedNote = Notes.FirstOrDefault(x => !x.IsBlank);
        _selectedNote?.Select();

        if (_selectedNote != null)
        {
            _selectedNote.PropertyChanged += OnNoteModelPropertyChanged;
        }

        if (_selectedNote is null)
        {
            _selectedNote = new NoteModel(_supportTool.Settings.UserSettings.ExtSeparator, _logger);
            Notes.Add(_selectedNote);
            _selectedNote.Select();
        }
    }

    private void OnNoteModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UiRefreshCounter++;
        ScheduleAutosave();
        _logger?.LogDebug($"Note property changed: {e.PropertyName} on {sender}");
    }

    [RelayCommand]
    private void OnNoteClicked(Guid caseId)
    {
        try
        {
            if (SelectedNote is null || caseId == SelectedNote.Id) return;

            foreach (var note in Notes.ToList())
            {
                if (note.Id == caseId)
                {
                    SelectedNote = note;
                    note.Select();
                    _logger?.LogInfo($"Note clicked and selected: {note.Id}");
                    continue;
                }
                note.Deselect();
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnNoteClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnDealerClicked(ISelectable company)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedDealer is null || 
                company.Id == SelectedNote.SelectedDealer.Id) return;

            SelectedNote.SelectDealer(company);
            _logger?.LogInfo($"Dealer clicked and selected: {company.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnDealerClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnCompanyClicked(ISelectable company)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedDealer is null || 
                SelectedNote.SelectedDealer.SelectedCompany is null ||
                company.Id == SelectedNote.SelectedDealer.SelectedCompany.Id) return;

            SelectedNote.SelectedDealer.SelectCompany(company);
            _logger?.LogInfo($"Company clicked and selected: {company.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnCompanyClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnContactClicked(ISelectable contact)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedContact is null ||
                contact.Id == SelectedNote.SelectedContact.Id) return;

            SelectedNote.SelectContact(contact);
            _logger?.LogInfo($"Contact clicked and selected: {contact.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnContactClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnFormClicked(ISelectable form)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedForm is null ||
                form.Id == SelectedNote.SelectedForm.Id) return;

            SelectedNote.SelectForm(form);
            _logger?.LogInfo($"Form clicked and selected: {form.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnFormClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnDealClicked(ISelectable deal)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedForm is null ||
                SelectedNote.SelectedForm.SelectedTestDeal is null ||
                deal.Id == SelectedNote.SelectedForm.SelectedTestDeal.Id) return;

            SelectedNote.SelectedForm.SelectTestDeal(deal);
            _logger?.LogInfo($"Deal clicked and selected: {deal.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnDealClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnDeleteItemClicked(object itemToDelete)
    {
        try
        {
            if (itemToDelete == null) return;

            if (itemToDelete is NoteModel noteToDelete)
            {
                bool isDeletingSelected = SelectedNote == noteToDelete;
                int deletedIndex = Notes.IndexOf(noteToDelete);

                // Only remove if more than one note exists, or if not blank
                if (Notes.Count > 1 || !noteToDelete.IsBlank)
                    Notes.Remove(noteToDelete);

                // Select next note by index, or previous if at end
                if (Notes.Count > 0)
                {
                    int nextIndex = Math.Min(deletedIndex, Notes.Count - 1);
                    SelectedNote = Notes[nextIndex];
                    SelectedNote?.Select();
                }
                else
                {
                    SelectedNote = null;
                }
                _logger?.LogInfo($"Note deleted: {noteToDelete.Id}");
            }
            else if (itemToDelete is Dealer dealerToDelete)
            {
                bool isDeletingSelected = SelectedNote.SelectedDealer == dealerToDelete;
                SelectedNote.Dealers.Remove(dealerToDelete);

                if (isDeletingSelected)
                {
                    SelectedNote.SelectDealer(SelectedNote.Dealers.FirstOrDefault(x => !x.IsBlank));
                    SelectedNote.SelectedDealer?.Select();
                }
                _logger?.LogInfo($"Dealer deleted: {dealerToDelete.Id}");
            }
            else if (itemToDelete is Company companyToDelete && SelectedNote.SelectedDealer is not null)
            {
                bool isDeletingSelected = SelectedNote.SelectedDealer.SelectedCompany == companyToDelete;
                SelectedNote.SelectedDealer.Companies.Remove(companyToDelete);

                if (isDeletingSelected)
                {
                    SelectedNote.SelectedDealer.SelectCompany(SelectedNote.SelectedDealer.Companies.FirstOrDefault(x => !x.IsBlank));
                    SelectedNote.SelectedDealer.SelectedCompany?.Select();
                }
                _logger?.LogInfo($"Company deleted: {companyToDelete.Id}");
            }
            else if (itemToDelete is Contact contactToDelete)
            {
                bool isDeletingSelected = SelectedNote.SelectedContact == contactToDelete;
                SelectedNote.Contacts.Remove(contactToDelete);

                if (isDeletingSelected)
                {
                    SelectedNote.SelectContact(SelectedNote.Contacts.FirstOrDefault(x => !x.IsBlank));
                    SelectedNote.SelectedContact?.Select();
                }
                _logger?.LogInfo($"Contact deleted: {contactToDelete.Id}");
            }
            else if (itemToDelete is Form formToDelete)
            {
                bool isDeletingSelected = SelectedNote.SelectedForm == formToDelete;
                SelectedNote.Forms.Remove(formToDelete);

                if (isDeletingSelected)
                {
                    SelectedNote.SelectForm(SelectedNote.Forms.FirstOrDefault(x => !x.IsBlank));
                    SelectedNote.SelectedForm?.Select();
                }
                _logger?.LogInfo($"Form deleted: {formToDelete.Id}");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnDeleteItemClicked.", ex);
        }
    }

    [RelayCommand]
    private void OpenTemplateDialog()
    {
        try
        {
            var vm = new TemplatesViewModel(_supportTool, _fileSystem);
            var page = new TemplatesPage(vm);
            var dialog = new PageHostDialog(page, "Templates");
            dialog.Show();
            _logger?.LogInfo("Opened Template Dialog.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error opening Template Dialog.", ex);
        }
    }

    [RelayCommand]
    private void OpenCodeSnippetDialog()
    {
        try
        {
            var vm = new CodeSnippetsViewModel(_supportTool);
            var page = new CodeSnippetsPage(vm);
            var dialog = new PageHostDialog(page, "Code Snippets");
            dialog.Show();
            _logger?.LogInfo("Opened Code Snippet Dialog.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error opening Code Snippet Dialog.", ex);
        }
    }

    [RelayCommand]
    private void OpenFormgenUtilsDialog()
    {
        try
        {
            var vm = new FormgenUtilitiesViewModel(_supportTool, _dialogService, _fileSystem);
            var navigationService = App.GetRequiredService<INavigationService>();
            var page = new FormgenUtilitiesPage(vm, navigationService, _logger);
            var dialog = new PageHostDialog(page, "Formgen Utilities");
            dialog.Show();
            _logger?.LogInfo("Opened Formgen Utilities Dialog.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error opening Formgen Utilities Dialog.", ex);
        }
    }

    [RelayCommand]
    private void OpenFormNameGeneratorDialog()
    {
        try
        {
            var vm = new FormNameGeneratorViewModel(_supportTool);
            var page = new FormNameGeneratorPage(vm); 
            var dialog = new PageHostDialog(page, "Form Name Generator", true);
            dialog.Show();
            dialog.Closed += FormNameDialogClosed;
            _logger?.LogInfo("Opened Form Name Generator Dialog.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error opening Form Name Generator Dialog.", ex);
        }
    }

    private void FormNameDialogClosed(object? sender, EventArgs e)
    {
        try
        {
            if (sender is null) return;

            var dialog = (PageHostDialog)sender;
            if (dialog.DataContext is not PageHostDialogViewModel outerVm) return;
            if (outerVm.HostedPageViewModel is not FormNameGeneratorViewModel vm) return;

            var name = vm.Form.FileName ?? string.Empty;

            if (dialog.ConfirmSelected)
                if(SelectedNote.SelectedForm.IsBlank)
                {
                    SelectedNote.SelectedForm.Name = name;
                }
                else
                {
                    SelectedNote.Forms.Last().Name = name;
                }
            dialog.Closed -= FormNameDialogClosed;
            _logger?.LogInfo("Form Name Dialog closed.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in FormNameDialogClosed.", ex);
        }
    }

    [RelayCommand]
    private void LoadCase()
    {
        //var dialog = new Dialogs.NewTemplateDialog();
        //dialog.ShowDialog();
        _logger?.LogInfo("LoadCase command executed.");
    }

    public void ToggleDebugMode(bool? debugModeEnable)
    {
        if (!debugModeEnable.HasValue)
            debugModeEnable = !IsDebugMode;

        IsDebugMode = debugModeEnable.Value;
        DebugVisibility = IsDebugMode ? Visibility.Visible : Visibility.Collapsed;
        _logger?.LogInfo($"Debug mode toggled: {IsDebugMode}");
    }

    public void TriggerRadioButtonRefresh()
    {
        UiRefreshCounter++;
        _logger?.LogDebug("RadioButton refresh triggered.");
    }

    private System.Timers.Timer? _debounceTimer;
    private const int DebounceIntervalMs = 5000; 

    private void ScheduleAutosave()
    {
        if (_debounceTimer == null)
        {
            _debounceTimer = new System.Timers.Timer(DebounceIntervalMs);
            _debounceTimer.Elapsed += (s, e) =>
            {
                _debounceTimer?.Stop();
                _debounceTimer?.Dispose();
                _debounceTimer = null; 
                IO.SaveNotes([.. Notes.Select(n => (Core.Types.Notebook.Note)n).Cast<INote>()]);
                _logger?.LogInfo("Autosave triggered.");
            };
            _debounceTimer.AutoReset = false;
        }
        else
        {
            _debounceTimer.Stop();
        }
        _debounceTimer.Start();
    }
}
