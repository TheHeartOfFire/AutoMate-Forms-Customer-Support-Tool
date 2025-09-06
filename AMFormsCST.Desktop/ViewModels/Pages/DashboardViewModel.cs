using AMFormsCST.Core;
using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.BaseClasses;
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
using System.Runtime.ExceptionServices;
using System.Windows;
using Wpf.Ui;

namespace AMFormsCST.Desktop.ViewModels.Pages;

public partial class DashboardViewModel : ViewModel
{
    private readonly ILogService _logger;
    private readonly IDebounceService _debounceService;

    private ManagedObservableCollection<NoteModel> _notes = new(() => new NoteModel(""));
    public ManagedObservableCollection<NoteModel> Notes => _notes;
    public NoteModel? SelectedNote => Notes.SelectedItem;

    private int _uiRefreshCounter;
    public int UiRefreshCounter
    {
        get => _uiRefreshCounter;
        private set => SetProperty(ref _uiRefreshCounter, value);
    }

    private readonly ISupportTool _supportTool;
    private readonly IDialogService _dialogService;
    private readonly IFileSystem _fileSystem;

    private NoteModel? _lastSelectedNote;
    private Models.Form? _lastSelectedForm;

    public DashboardViewModel(ISupportTool supportTool, IDialogService dialogService, IFileSystem fileSystem, ILogService logger, IDebounceService debounceService)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _fileSystem = fileSystem;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInfo("DashboardViewModel initialized.");
        _debounceService = debounceService;
        _debounceService.DebouncedElapsed += AutosaveTimerElapsed;

        void postCreationAction(NoteModel note)
        {
            note.Parent = this;
            note.PropertyChanged += OnNoteModelPropertyChanged;
        }

        // Populate Notes collection
        if (_supportTool.Notebook.Notes.Count == 0)
        {
            var initialNote = new NoteModel(_supportTool.Settings.UserSettings.ExtSeparator, _logger);
            postCreationAction(initialNote);

            _notes = new ManagedObservableCollection<NoteModel>(
                () => new NoteModel(_supportTool.Settings.UserSettings.ExtSeparator, _logger),
                [initialNote],
                _logger,
                postCreationAction
            );
        }
        else
        {
            var notes = _supportTool.Notebook.Notes
                .Select(note =>
                {
                    var noteModel = new NoteModel(note, _supportTool.Settings.UserSettings.ExtSeparator, _logger);
                    postCreationAction(noteModel);
                    return noteModel;
                });

            _notes = new ManagedObservableCollection<NoteModel>(
                () => new NoteModel(_supportTool.Settings.UserSettings.ExtSeparator, _logger),
                notes,
                _logger,
                postCreationAction
            );
        }

        _notes.PropertyChanged += Notes_PropertyChanged;
        // Initial subscription
        if (_notes.SelectedItem is not null)
        {
            Notes_PropertyChanged(this, new PropertyChangedEventArgs(nameof(Notes.SelectedItem)));
        }
    }

    private void AutosaveTimerElapsed(object? sender, EventArgs e) => IO.SaveNotes([.. Notes.Select(n => n.CoreType).Cast<INote>()]);

    private void Notes_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ManagedObservableCollection<NoteModel>.SelectedItem)) return;

        if (_lastSelectedNote != null)
        {
            _lastSelectedNote.Dealers.PropertyChanged -= SelectedNote_Dealers_PropertyChanged;
            _lastSelectedNote.Contacts.PropertyChanged -= SelectedNote_Contacts_PropertyChanged;
            _lastSelectedNote.Forms.PropertyChanged -= SelectedNote_Forms_PropertyChanged;
        }

        OnPropertyChanged(nameof(SelectedNote));

        if (SelectedNote != null)
        {
            SelectedNote.Dealers.PropertyChanged += SelectedNote_Dealers_PropertyChanged;
            SelectedNote.Contacts.PropertyChanged += SelectedNote_Contacts_PropertyChanged;
            SelectedNote.Forms.PropertyChanged += SelectedNote_Forms_PropertyChanged;
        }

        _lastSelectedNote = SelectedNote;
    }

    private void SelectedNote_Dealers_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ManagedObservableCollection<Models.Dealer>.SelectedItem))
        {
            OnPropertyChanged(nameof(SelectedNote));
        }
    }

    private void SelectedNote_Contacts_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ManagedObservableCollection<Models.Contact>.SelectedItem))
        {
            OnPropertyChanged(nameof(SelectedNote));
        }
    }

    private void SelectedNote_Forms_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ManagedObservableCollection<Models.Form>.SelectedItem)) return;
        
        if (_lastSelectedForm != null)
        {
            _lastSelectedForm.TestDeals.PropertyChanged -= SelectedForm_TestDeals_PropertyChanged;
        }

        OnPropertyChanged(nameof(SelectedNote));

        if (SelectedNote?.SelectedForm != null)
        {
            SelectedNote.SelectedForm.TestDeals.PropertyChanged += SelectedForm_TestDeals_PropertyChanged;
        }

        _lastSelectedForm = SelectedNote?.SelectedForm;
    }

    private void SelectedForm_TestDeals_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ManagedObservableCollection<Models.TestDeal>.SelectedItem))
        {
            OnPropertyChanged(nameof(SelectedNote));
        }
    }

    private void OnNoteModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        UiRefreshCounter++;
        _debounceService.ScheduleEvent();
        _logger?.LogDebug($"Note property changed: {e.PropertyName} on {sender}");
    }

    [RelayCommand]
    private void OnNoteClicked(Guid caseId)
    {
        try
        {
            if (Notes.SelectedItem is null || caseId == Notes.SelectedItem.Id) return;

            Notes.First(x => x.Id == caseId).Select();

            _logger?.LogInfo($"Note clicked and selected: {caseId}");

        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnNoteClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnDealerClicked(Models.Dealer dealer)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedDealer is null ||
                dealer.Id == SelectedNote.SelectedDealer.Id) return;
            dealer.Select();

            _logger?.LogInfo($"Dealer clicked and selected: {dealer.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnDealerClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnCompanyClicked(Models.Company company)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedDealer is null ||
                SelectedNote.SelectedDealer.SelectedCompany is null ||
                company.Id == SelectedNote.SelectedDealer.SelectedCompany.Id) return;

            company.Select();

            _logger?.LogInfo($"Company clicked and selected: {company.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnCompanyClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnContactClicked(Models.Contact contact)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedContact is null ||
                contact.Id == SelectedNote.SelectedContact.Id) return;

            contact.Select();
            _logger?.LogInfo($"Contact clicked and selected: {contact.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnContactClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnFormClicked(Models.Form form)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedForm is null ||
                form.Id == SelectedNote.SelectedForm.Id) return;

            form.Select();
            _logger?.LogInfo($"Form clicked and selected: {form.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnFormClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnDealClicked(Models.TestDeal deal)
    {
        try
        {
            if (SelectedNote is null ||
                SelectedNote.SelectedForm is null ||
                SelectedNote.SelectedForm.SelectedTestDeal is null ||
                deal.Id == SelectedNote.SelectedForm.SelectedTestDeal.Id) return;

            deal.Select();
            _logger?.LogInfo($"Deal clicked and selected: {deal.Id}");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error in OnDealClicked.", ex);
        }
    }

    [RelayCommand]
    private void OnDeleteItemClicked(ManagedObservableCollectionItem itemToDelete)
    {
        try
        {
            if (itemToDelete == null) return;

            switch (itemToDelete)
            {
                case NoteModel noteToDelete:
                    Notes.Remove(noteToDelete);
                    _logger?.LogInfo($"Note deleted: {noteToDelete.Id}");
                    break;

                case Models.Dealer dealerToDelete when SelectedNote is not null:
                    SelectedNote.Dealers.Remove(dealerToDelete);
                    _logger?.LogInfo($"Dealer deleted: {dealerToDelete.Id}");
                    break;

                case Models.Company companyToDelete when
                SelectedNote is not null &&
                SelectedNote.SelectedDealer is not null:
                    SelectedNote.SelectedDealer.Companies.Remove(companyToDelete);
                    _logger?.LogInfo($"Company deleted: {companyToDelete.Id}");
                    break;

                case Models.Contact contactToDelete when SelectedNote is not null:
                    SelectedNote.Contacts.Remove(contactToDelete);
                    _logger?.LogInfo($"Contact deleted: {contactToDelete.Id}");
                    break;

                case Models.Form formToDelete when SelectedNote is not null:
                    SelectedNote.Forms.Remove(formToDelete);
                    _logger?.LogInfo($"Form deleted: {formToDelete.Id}");
                    break;

                case Models.TestDeal dealToDelete when
                SelectedNote?.SelectedForm is not null:
                    SelectedNote.SelectedForm.TestDeals.Remove(dealToDelete);
                    _logger?.LogInfo($"TestDeal deleted: {dealToDelete.Id}");
                    break;
            }

            _debounceService.ScheduleEvent();
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
                if (SelectedNote?.SelectedForm?.IsBlank == true)
                {
                    SelectedNote.SelectedForm.Name = name;
                }
                else if (SelectedNote is not null)
                {
                    SelectedNote.Forms.Last().Name = name ?? string.Empty;
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

    //public void TriggerRadioButtonRefresh()
    //{
    //    UiRefreshCounter++;
    //    _logger?.LogDebug("RadioButton refresh triggered.");
    //}
}
