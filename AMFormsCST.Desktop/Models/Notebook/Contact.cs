using AMFormsCST.Core.Interfaces.Notebook;
using AMFormsCST.Core.Types.Notebook;
using AMFormsCST.Desktop.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public partial class Contact : ObservableObject, ISelectable, IBlankMaybe
{
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _email = string.Empty;
    [ObservableProperty]
    private string _phone = string.Empty;
    [ObservableProperty]
    private string _phoneExtension = string.Empty;
    [ObservableProperty]
    private string _phoneExtensionDelimiter = " ";
    public Guid Id { get; } = Guid.NewGuid();

    [ObservableProperty]
    private bool _isSelected = false;
    internal IContact CoreType = new Core.Types.Notebook.Contact();
    public Contact(string extensionDelimiter)
    {
        PhoneExtensionDelimiter = extensionDelimiter;
    }
    public Contact(IContact contact)
    {
        CoreType = contact ?? throw new ArgumentNullException(nameof(contact), "Cannot create a Contact from a null item."); ;
        Name = contact.Name ?? string.Empty;
        Email = contact.Email ?? string.Empty;
        Phone = contact.Phone ?? string.Empty;
        PhoneExtension = contact.PhoneExtension ?? string.Empty;
        PhoneExtensionDelimiter = contact.PhoneExtensionDelimiter;

    }
    public void Select()
    {
        IsSelected = true;
    }
    public void Deselect()
    {
        IsSelected = false;
    }

    public string FullPhone
    {
        get
        {
            if (string.IsNullOrEmpty(Phone))
                return string.Empty;
            if (string.IsNullOrEmpty(PhoneExtension))
                return Phone;
            return $"{Phone}{PhoneExtensionDelimiter}{PhoneExtension}";
        }
    }

    public void ParsePhone(string phone)
    { 
        // TODO: This is not robust enough. It should parse all sorts of phone numbers.
        if (string.IsNullOrEmpty(phone))
            return;
        var parts = phone.Split(' ');
        Phone = parts[0];
        if (parts.Length > 1)
            PhoneExtension = parts[1];
    }
    public bool IsBlank { get { return string.IsNullOrEmpty(Name) &&
                                 string.IsNullOrEmpty(Email) &&
                                 string.IsNullOrEmpty(Phone) &&
                                 string.IsNullOrEmpty(PhoneExtension); } }
    partial void OnNameChanged(string value) { OnPropertyChanged(nameof(IsBlank)); }
    partial void OnEmailChanged(string value) { OnPropertyChanged(nameof(IsBlank)); }
    partial void OnPhoneChanged(string value) 
    { 
        OnPropertyChanged(nameof(IsBlank));
        OnPropertyChanged(nameof(FullPhone));
    } 
    partial void OnPhoneExtensionChanged(string value) 
    { 
        OnPropertyChanged(nameof(IsBlank));
        OnPropertyChanged(nameof(FullPhone));
    }
    partial void OnPhoneExtensionDelimiterChanged(string value)
    {
        OnPropertyChanged(nameof(FullPhone));
    }
    public static implicit operator Core.Types.Notebook.Contact(Contact contact)
    {
        if (contact is null) return new Core.Types.Notebook.Contact();

        return new Core.Types.Notebook.Contact(contact.Id)
        {
            Name = contact.Name ?? string.Empty,
            Email = contact.Email ?? string.Empty,
            Phone = contact.Phone ?? string.Empty,
            PhoneExtension = contact.PhoneExtension ?? string.Empty,
            PhoneExtensionDelimiter = contact.PhoneExtensionDelimiter ?? " "
        };
    }
}
