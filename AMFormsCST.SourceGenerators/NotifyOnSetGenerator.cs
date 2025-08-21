using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Generator]
public class NotifyOnSetGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this simple generator
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Find all classes with properties marked [NotifyOnSet]
        var syntaxTrees = context.Compilation.SyntaxTrees;
        foreach (var tree in syntaxTrees)
        {
            var semanticModel = context.Compilation.GetSemanticModel(tree);
            var classNodes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classNode in classNodes)
            {
                var properties = classNode.Members.OfType<PropertyDeclarationSyntax>()
                    .Where(p => p.AttributeLists
                        .SelectMany(a => a.Attributes)
                        .Any(attr => semanticModel.GetTypeInfo(attr).Type?.Name == "NotifyOnSetAttribute"));

                if (!properties.Any())
                    continue;

                var className = classNode.Identifier.Text;
                var sb = new StringBuilder();
                sb.AppendLine($"partial class {className} : System.ComponentModel.INotifyPropertyChanged {{");
                sb.AppendLine("public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;");
                sb.AppendLine("protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));");

                foreach (var prop in properties)
                {
                    var propType = prop.Type.ToString();
                    var propName = prop.Identifier.Text;
                    var fieldName = $"_{char.ToLower(propName[0])}{propName.Substring(1)}";
                    sb.AppendLine($"private {propType} {fieldName};");
                    sb.AppendLine($"public {propType} {propName} {{");
                    sb.AppendLine($"    get => {fieldName};");
                    sb.AppendLine($"    set {{ {fieldName} = value; OnPropertyChanged(nameof({propName})); }}");
                    sb.AppendLine("}");
                }
                sb.AppendLine("}");

                context.AddSource($"{className}_NotifyOnSet.g.cs", sb.ToString());
            }
        }
    }
}