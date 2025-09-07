using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop.Types
{
    public class GuidEventArgs(Guid value) : System.EventArgs
    {
        public Guid Value { get; } = value;
    }
}
