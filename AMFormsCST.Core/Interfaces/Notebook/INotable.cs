using AMFormsCST.Core.Types.Notebook;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFormsCST.Core.Interfaces.Notebook;
public interface INotable<T> : IEquatable<T>, IEqualityComparer<T>
{
    Guid Id { get; }
    new bool Equals(T? other);
    new bool Equals(T? x, T? y);
    bool Equals(object? obj);
    int GetHashCode();
    new int GetHashCode([DisallowNull] T obj);
    string Dump();
    T Clone();
    
}
