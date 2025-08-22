using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.Notebook;
public interface ICompany : INotable<ICompany>
{
    string Name { get; set; }
    string CompanyCode { get; set; }
    bool Notable { get; set; }
}
