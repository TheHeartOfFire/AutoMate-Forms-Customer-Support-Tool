using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Utils;
public class BestPracticeEnforcer(IFormNameBestPractice formNameBestPractice)
{
    public static readonly IReadOnlyList<string> StateCodes = ["AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "VI", "WA", "WV", "WI", "WY"];
    public IFormNameBestPractice FormNameBestPractice { get; private set; } = formNameBestPractice;
    public string GetFormName() => FormNameBestPractice.Generate();
    public List<TextTemplate> Templates { get; } = [];

}
