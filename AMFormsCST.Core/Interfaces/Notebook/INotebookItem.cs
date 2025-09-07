using System;
using System.Diagnostics.CodeAnalysis;

namespace AMFormsCST.Core.Interfaces.Notebook;

public interface INotebookItem<T> : IEquatable<T> where T : class
{
    Guid Id { get; }
    string Dump();
    T Clone();
}