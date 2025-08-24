using AMFormsCST.Core.Types.Notebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.Notebook;
[JsonDerivedType(typeof(Contact), typeDiscriminator: "contact")]
public interface IContact : INotable<IContact>
{
    string Name { get; set; }
    string Email { get; set; } 
    string Phone { get; set; } 
    string PhoneExtension { get; set; }
    string PhoneExtensionDelimiter { get; set; }
}
