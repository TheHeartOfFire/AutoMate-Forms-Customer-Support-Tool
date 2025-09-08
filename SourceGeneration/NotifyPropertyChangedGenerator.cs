using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text;

namespace SourceGeneration;

[Generator]
public class NotifyPropertyChangedGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
            .Where(static classNode =>
                classNode.Members.OfType<FieldDeclarationSyntax>()
                    .Any(field => field.AttributeLists
                        .SelectMany(a => a.Attributes)
                        .Any(attr => attr.Name.ToString().Contains("NotifyPropertyChanged"))));

        context.RegisterSourceOutput(classDeclarations, (spc, classNode) =>
        {
            var ns = classNode.Parent as NamespaceDeclarationSyntax;
            var namespaceName = ns != null ? ns.Name.ToString() : null;

            var className = classNode.Identifier.Text;
            var fields = classNode.Members.OfType<FieldDeclarationSyntax>()
                .Where(field => field.AttributeLists
                    .SelectMany(a => a.Attributes)
                    .Any(attr => attr.Name.ToString().Contains("NotifyPropertyChanged")));

            var sb = new StringBuilder();
            sb.AppendLine("using System.Drawing;");
            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine($"namespace {namespaceName};");
            }
            sb.AppendLine($"partial class {className} {{");
            sb.AppendLine("    public event System.EventHandler? PropertyChanged;");
            sb.AppendLine("    public void OnPropertyChanged() => PropertyChanged?.Invoke(this, EventArgs.Empty);");

            foreach (var field in fields)
            {
                var variable = field.Declaration.Variables.First();
                var fieldType = field.Declaration.Type.ToString();
                var fieldName = variable.Identifier.Text;

                // Convert _name to Name (PascalCase, remove leading underscore)
                var propName = fieldName.StartsWith("_") && fieldName.Length > 1
                    ? char.ToUpper(fieldName[1]) + fieldName.Substring(2)
                    : char.ToUpper(fieldName[0]) + fieldName.Substring(1);

                sb.AppendLine($"    public {fieldType} {propName} {{");
                sb.AppendLine($"        get => {fieldName};");
                sb.AppendLine($"        set {{ {fieldName} = value; OnPropertyChanged(); }}");
                sb.AppendLine("    }");
            }
            sb.AppendLine("}");

            spc.AddSource($"{className}_NotifyPropertyChanged.g.cs", sb.ToString());
        });
    }
}
