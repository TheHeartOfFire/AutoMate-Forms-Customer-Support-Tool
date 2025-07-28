using AMFormsCST.Core.Interfaces.BestPractices;
using AMFormsCST.Core.Interfaces.Utils;
using AMFormsCST.Core.Types.BestPractices.TextTemplates.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Utils;
public class BestPracticeEnforcer(IFormNameBestPractice formNameBestPractice) : IBestPracticeEnforcer
{
    public static readonly IReadOnlyList<string> StateCodes = ["AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "VI", "WA", "WV", "WI", "WY"];
    public IFormNameBestPractice FormNameBestPractice { get; private set; } = formNameBestPractice;
    public string GetFormName() => FormNameBestPractice.Generate();
    public List<TextTemplate> Templates { get; } = IO.LoadTemplates();

    public void AddTemplate(TextTemplate template)
    {
        ArgumentNullException.ThrowIfNull(template);
        if (string.IsNullOrWhiteSpace(template.Text)) throw new ArgumentException("Template text cannot be null or whitespace.", nameof(template));
        if(Templates.Contains(template)) throw new ArgumentException("Template already exists.", nameof(template));

        Templates.Add(template);
        IO.SaveTemplates(Templates);
    }

    public void RemoveTemplate(TextTemplate template)
    {
        ArgumentNullException.ThrowIfNull(template);
        if (string.IsNullOrWhiteSpace(template.Text)) throw new ArgumentException("Template text cannot be null or whitespace.", nameof(template));
        if (!Templates.Contains(template)) throw new ArgumentException("Template does not exist.", nameof(template));

        Templates.Remove(template);
        IO.SaveTemplates(Templates);
    }
    public void UpdateTemplate(TextTemplate updatedTemplate)
    {
        // Find the existing template by its ID
        var existingTemplate = Templates.FirstOrDefault(t => t.Id == updatedTemplate.Id);
        if (existingTemplate != null)
        {
            // Update its properties directly
            existingTemplate.Name = updatedTemplate.Name;
            existingTemplate.Description = updatedTemplate.Description;
            existingTemplate.Text = updatedTemplate.Text;

            IO.SaveTemplates(Templates);
        }
    }
}
