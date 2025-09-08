namespace AMFormsCST.Core.Types.CodeBlocks.Functions
{
    public class ContainsCode : CodeBase
    {
        public ContainsCode()
        {
            Name = "Contains";
            Prefix = "CONTAINS";
            Description = "Returns TRUE if TextB appears anywhere within TextA";
            AddInput("TextA");
            AddInput("TextB");
        }
    }
}
