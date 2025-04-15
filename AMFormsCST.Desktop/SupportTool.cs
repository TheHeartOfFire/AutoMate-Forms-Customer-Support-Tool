using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types.BestPractices.Models;
using AMFormsCST.Core.Types.BestPractices.Practices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Desktop
{
    internal static class SupportTool
    {
        internal static ISupportTool SupportToolInstance = new Core.SupportTool( 
            new AutoMateFormNameBestPractices(
                new AutoMateFormModel()));
    }
}
