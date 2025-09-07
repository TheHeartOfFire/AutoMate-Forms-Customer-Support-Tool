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
    private readonly ILogService? _logger;
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

    public DashboardViewModel(ISupportTool supportTool, IDialogService dialogService, IFileSystem fileSystem, IDebounceService debounceService, ILogService? logger = null)
    {
        _supportTool = supportTool ?? throw new ArgumentNullException(nameof(supportTool));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _fileSystem = fileSystem;
        _logger = logger;
        _logger?.LogInfo("DashboardViewModel initialized.");
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
        var note = ParseCaseText(Clipboard.GetText());
        Notes.Add(note);
        note.Select();

        _logger?.LogInfo("LoadCase command executed.");
    }

    public NoteModel ParseCaseText(string caseText)
    {
        var note = new NoteModel(_supportTool.Settings.UserSettings.ExtSeparator, _logger);
        var textLines = caseText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        try
        {
            note.ParseCaseText(caseText);
            _logger?.LogInfo("Case text parsed into NoteModel.");
        }
        catch (Exception ex)
        {
            _logger?.LogError("Error parsing case text.", ex);
            ExceptionDispatchInfo.Capture(ex).Throw();
        }
        return note;
    }

    //private void btnLoad_Click(object sender, RoutedEventArgs e)
    //{
    //    var text = Clipboard.GetText();

    //    if (string.IsNullOrEmpty(text)) return;

    //    var lines = text.Split('\n');

    //    if (!lines[0].Equals("Case Number\r")) return;

    //    var newNote = LoadSFNotes(lines);

    //    NotesList.Add(newNote);
    //    tcTabs.Items.Add(newNote.TabItem);
    //    tcTabs.SelectedItem = newNote.TabItem;
    //    ToggleClose();

    //    Clipboard.Clear();
    //}

    //private NotesInfo LoadSFNotes(string[] notes)
    //{
    //    var submittedIdx = Array.IndexOf(notes, "Submitted Values:\r");
    //    var serverIdx = Array.IndexOf(notes, "Server-Provided Values:\r");
    //    string[]? submittedValues = null;
    //    string[]? serverValues = null;

    //    if (submittedIdx > 0)
    //        submittedValues = notes[submittedIdx..];

    //    if (submittedIdx > 0 && serverIdx > 0)
    //        submittedValues = notes[submittedIdx..serverIdx];

    //    if (serverIdx > 0)
    //        serverValues = notes[serverIdx..];

    //    NotesInfo newNote = new(NotesList[0].TabItem.Clone())
    //    {
    //        CaseText = notes.Length >= 1 ? notes[1].Trim() : string.Empty,
    //        ContactName = notes.Length >= 10 ? notes[10].Trim() : string.Empty,
    //        Email = submittedValues is not null && submittedValues.Length >= 3 ? submittedValues[3][7..].Trim() : string.Empty,
    //        Phone = submittedValues is not null && submittedValues.Length >= 4 ? submittedValues[4][6..].Trim() : string.Empty,
    //        Companies = submittedValues is not null && submittedValues.Length >= 5 ? submittedValues[5][15..].Trim() : string.Empty,
    //        Dealership = submittedValues is not null && submittedValues.Length >= 6 ? submittedValues[6][13..].Trim() : string.Empty,
    //        ServerId = serverValues is not null && serverValues.Length >= 2 ? serverValues[2][10..].Trim() : string.Empty
    //    };

    //    return newNote;
    //}

    /* no mistakes in parsing from depricated setup
     * Case Number
    12453488
    Case Owner
    Dakota Jordan
    Dakota Jordan
    Status
    New
    Priority
    Standard
    Contact Name
    Danielle Johnson
    Subject
    new forms
    Description
    Please add the attached forms.

    ====================

    Submitted Values:
    Name: Danielle Johnson
    Title: Office Manager
    Email: daniellejohnson@wagnercadillac.com
    Phone: 9035611212
    Company Number: 4
    Company Name: Wagner Cadillac

    Server-Provided Values:
    HAC: HW8F760K
    Server ID: G030
    Username: danij
    Name: Danielle Johnson
    Email: daniellejohnson@wagnercadillac.com

    Versions:
    AMPS: 3.06.0386
    Tomcat: 3.6.389a
    Web Browser: 11

    // Name = 'Subject'

        Case Number
    12455241
    Case Owner
    Dakota Jordan
    Status
    In Progress
    Priority
    Standard
    Contact Name
    Rachel Gause
    Subject
    title app
    Description
    please add michigan title app

    ====================

    Submitted Values:
    Name: RACHEL A. GAUSE
    Title: manager
    Email: rachel@carolinaautodirect.com
    Phone: 9802812984
    Company Number: 1
    Company Name: Carolina Auto Direct

    Server-Provided Values:
    HAC: HDQ521V1
    Server ID: T751
    Username: rachelg
    Name: RACHEL A. GAUSE
    Email: rachel@carolinaautodirect.com

    Versions:
    AMPS: 3.06.0386
    Tomcat: 3.6.389a
    Web Browser: 11

    no mistakes in parsing from depricated setup
     

    Case Number
12459417
Case Owner
Dakota Jordan
Dakota Jordan
Status
In Progress
Priority
Standard
Contact Name
Dorothy Davis
Subject
SC 5047 Form
Description
The Seller/Transferor's Name is not printing on line properly in Part A.
Also, in Part C, the Person excercising power of attorney line should be blank.
Please assist.

====================

Submitted Values:
Name: Dorothy Davis
Title: Business Manager
Email: danielled@mrchevrolet.com
Phone: 8432088832
Company Number: 3
Company Name: Mike Reichenbach Chevrolet

Server-Provided Values:
HAC: HR1GTF01
Server ID: M450
Username: danielle
Name: Dorothy Davis
Email: DDDAVIS812@GMAIL.COM

Versions:
AMPS: 3.06.0386
Tomcat: 3.6.389a
Web Browser: 11
    */
}
