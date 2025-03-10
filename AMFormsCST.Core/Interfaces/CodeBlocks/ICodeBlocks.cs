namespace AMFormsCST.Core.Interfaces.CodeBlocks;
public interface ICodeBlocks
{
    IList<ICodeBase> CustomBlocks { get; set; }

    IReadOnlyCollection<ICodeBase> GetBlocks();
}