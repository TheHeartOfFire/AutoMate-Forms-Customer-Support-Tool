namespace AMFormsCST.Desktop.Models.FormgenUtilities;

public class DisplayProperty(string name, string? value)
{
    public string Name { get; } = name;
    public string? Value { get; } = value;
}