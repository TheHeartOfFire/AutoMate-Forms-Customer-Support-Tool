using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public class Contact
{
    public string? Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? PhoneExtension { get; set; } = string.Empty;
    public string? PhoneExtensionDelimiter { get; set; } = " ";

    public string GetPhone()
    {
        if (string.IsNullOrEmpty(Phone))
            return string.Empty;
        if (string.IsNullOrEmpty(PhoneExtension))
            return Phone;
        return $"{Phone}{PhoneExtensionDelimiter}{PhoneExtension}";
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
}
