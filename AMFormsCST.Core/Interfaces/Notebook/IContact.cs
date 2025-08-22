using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.Notebook;
public interface IContact : INotable<IContact>
{
    string Name { get; set; }
    string Email { get; set; } 
    string Phone { get; set; } 
    string PhoneExtension { get; set; }
    string PhoneExtensionDelimiter { get; set; }
}
