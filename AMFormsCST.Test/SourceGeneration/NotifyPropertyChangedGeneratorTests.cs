using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGeneration;
using System.Linq;
using Xunit;

namespace SourceGeneration.Tests;

public class NotifyPropertyChangedGeneratorTests
{
    [Fact]
    public void Generator_AddsPropertyWithNotification_ForDecoratedFields()
    {
        // Arrange
        var source = @"
using System;
namespace TestNamespace
{
    public partial class TestClass
    {
        [NotifyPropertyChanged]
        private int _value;
    }
}
";
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location)
        };
        var compilation = CSharpCompilation.Create("TestAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new NotifyPropertyChangedGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Act
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
        var generatedTrees = outputCompilation.SyntaxTrees.Skip(1).ToList(); // Skip the original source

        // Assert
        Assert.Single(generatedTrees);
        var generatedCode = generatedTrees[0].ToString();
        Assert.Contains("public int Value", generatedCode);
        Assert.Contains("set { _value = value; OnPropertyChanged(); }", generatedCode);
        Assert.Contains("public event System.EventHandler? PropertyChanged;", generatedCode);
        Assert.Contains("public void OnPropertyChanged()", generatedCode);
    }

    [Fact]
    public void Generator_UsesCorrectNamespace()
    {
        // Arrange
        var source = @"
using System;
namespace CustomNamespace
{
    public partial class CustomClass
    {
        [NotifyPropertyChanged]
        private string _name;
    }
}
";
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location)
        };
        var compilation = CSharpCompilation.Create("TestAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new NotifyPropertyChangedGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Act
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
        var generatedTrees = outputCompilation.SyntaxTrees.Skip(1).ToList();

        // Assert
        Assert.Single(generatedTrees);
        var generatedCode = generatedTrees[0].ToString();
        Assert.Contains("namespace CustomNamespace;", generatedCode);
        Assert.Contains("public string Name", generatedCode);
    }

    [Fact]
    public void Generator_IgnoresFieldsWithoutAttribute()
    {
        // Arrange
        var source = @"
using System;
namespace TestNamespace
{
    public partial class TestClass
    {
        private int _value;
    }
}
";
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location)
        };
        var compilation = CSharpCompilation.Create("TestAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new NotifyPropertyChangedGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Act
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
        var generatedTrees = outputCompilation.SyntaxTrees.Skip(1).ToList();

        // Assert
        Assert.Empty(generatedTrees);
    }
}