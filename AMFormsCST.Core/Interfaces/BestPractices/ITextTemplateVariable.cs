namespace AMFormsCST.Core.Interfaces.BestPractices;

public interface ITextTemplateVariable
{
    IReadOnlyCollection<string> Aliases { get; }
    string Name { get; }
    string Prefix { get; }
    string ProperName { get; }

    string GetValue();

}