namespace AMFormsCST.Core.Types.CodeBlocks.Functions
{
    public class TextCode : CodeBase
    {
        public TextCode()
        {
            Name = "Text";
            Prefix = "TEXT";
            Description = "Convert a Date or Numeric field to a Text field";
            AddInput("Value");
        }
    }
}
