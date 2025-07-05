using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Models;
public class Company : ISelectable
{
    public string? Name { get; set; } = string.Empty;
    public string? ServerCode { get; set; } = string.Empty;
    public string? CompanyCode { get; set; } = string.Empty;
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
}
