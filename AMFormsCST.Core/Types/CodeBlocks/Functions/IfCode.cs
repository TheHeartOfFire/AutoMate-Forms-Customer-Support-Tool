namespace AMFormsCST.Core.Types.CodeBlocks.Functions
{
    public class IfCode : CodeBase
    {
        public IfCode()
        {
            Name = "If Statement";
            Prefix = "IF";
            Description = "If the condition is true, return the true value, otherwise return the false value.";

            AddInput("Condition");
            AddInput("ResultIfTrue");
            AddInput("ResultIfFalse");
        }
    }
}
