using AMFormsCST.Core.Types.CodeBlocks;

namespace AMFormsCST.Core.Interfaces.FormgenUtils
{
    public interface IExtendableCode
    {
        int DefaultArgCount { get; }
        int ArgIncrement { get; }
        CodeBase AddExtraInputs(int count);
        CodeBase RemoveExtraInputs(int count);
    }
}
