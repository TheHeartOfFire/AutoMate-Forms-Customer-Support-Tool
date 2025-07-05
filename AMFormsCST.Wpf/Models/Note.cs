using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public class Note : ISelectable
{
    public string? CaseNumber { get; set; }
    public string? Notes { get; set; }
    public ObservableCollection<Company> Companies { get; set; } = [ new() ];
    public Company? SelectedCompany { get; set; }
    public ObservableCollection<Contact> Contacts { get; set; } = [ new() ];
    public Contact? SelectedContact { get; set; }
    public ObservableCollection<Form> Forms { get; set; } = [ new() ];
    public Form? SelectedForm { get; set; }
    public Guid Id { get; } = Guid.NewGuid(); 
    public bool IsSelected { get; private set; } = false;
    public void Select()
    {
        IsSelected = true;
    }
    public void Deselect()
    {
        IsSelected = false;
    }

    public void SelectCompany(ISelectable contact)
    {
        if (contact is null) return;

        foreach (var dealer in Companies) dealer.Deselect();

        SelectedCompany = Companies.FirstOrDefault(c => c.Id == contact.Id);
        SelectedCompany?.Select();
    }

    public void SelectContact(ISelectable contact)
    {
        if (contact is null) return;

        foreach (var dealer in Companies) dealer.Deselect();

        SelectedContact = Contacts.FirstOrDefault(c => c.Id == contact.Id);
        SelectedContact?.Select();
    }

    public void SelectForm(ISelectable form)
    {
        if (form is null) return;

        foreach (var dealer in Companies) dealer.Deselect();

        SelectedForm = Forms.FirstOrDefault(c => c.Id == form.Id);
        SelectedForm?.Select();
    }
}
