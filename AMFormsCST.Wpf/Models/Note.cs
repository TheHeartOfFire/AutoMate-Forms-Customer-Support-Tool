using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
internal class Note
{
    public string? CaseNumber { get; set; }
    public string? Notes { get; set; }
    public ObservableCollection<Company> Companies { get; set; } = [ new() ];
    public ObservableCollection<Contact> Contacts { get; set; } = [ new() ];
    public ObservableCollection<Form> Forms { get; set; } = [ new() ];
}
