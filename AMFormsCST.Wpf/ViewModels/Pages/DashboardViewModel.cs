using AMFormsCST.Core.Utils;
using AMFormsCST.Desktop.Interfaces;
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
        Notes[0].Select();
        SelectedNote.Companies.Clear();
        SelectedNote.Companies.Add(new Company { Name = "Default Company 1" });
        SelectedNote.Companies.Add(new Company { Name = "Default Company 2" });
        SelectedNote.Companies.Add(new Company { Name = "Default Company 3" });
        SelectedNote.SelectedCompany = SelectedNote!.Companies[0];
        SelectedNote.SelectedCompany.Select();
        SelectedNote.SelectedCompany!.CompanyCode = "1";
        SelectedNote.Contacts.Clear();
        SelectedNote.Contacts.Add(new Contact { Name = "Contact 1" });
        SelectedNote.Contacts.Add(new Contact { Name = "Contact 2" });
        SelectedNote.Contacts.Add(new Contact { Name = "Contact 3" });
        SelectedNote.SelectedContact = SelectedNote.Contacts[0];
        SelectedNote.SelectedContact.Select();
        SelectedNote.Forms.Clear();
        SelectedNote.Forms.Add(new Form { Name = "Form 1" });
        SelectedNote.Forms.Add(new Form { Name = "Form 2" });
        SelectedNote.Forms.Add(new Form { Name = "Form 3" });
        SelectedNote.SelectedForm = SelectedNote.Forms[0];
        SelectedNote.SelectedForm.Select();
        if (IsDebugMode) _debugVisibility = Visibility.Visible;
    }

    [RelayCommand]
    private void OnNoteClicked(Guid caseId)
    {
        if (SelectedNote is null || 
            caseId == SelectedNote.Id) return;

        foreach (var note in Notes)
        {
            if (note.Id == caseId)
            {
                SelectedNote = note;
                note.Select();
                continue;
            }
            note.Deselect();
        }
    }

    [RelayCommand]
    private void OnDealerClicked(ISelectable company)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedCompany is null || 
            company.Id == SelectedNote.SelectedCompany.Id) return;

        SelectedNote.SelectCompany(company);
    }

    [RelayCommand]
    private void OnCompanyClicked(ISelectable company)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedCompany is null ||
            company.Id == SelectedNote.SelectedCompany.Id) return;

        SelectedNote.SelectCompany(company);
    }

    [RelayCommand]
    private void OnContactClicked(ISelectable contact)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedContact is null ||
            contact.Id == SelectedNote.SelectedContact.Id) return;

        SelectedNote.SelectContact(contact);
    }

    [RelayCommand]
    private void OnFormClicked(ISelectable form)
    {
        if (SelectedNote is null ||
            SelectedNote.SelectedForm is null ||
            form.Id == SelectedNote.SelectedForm.Id) return;

        SelectedNote.SelectForm(form);
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