namespace AMFormsCST.Core.Types.CodeBlocks
{
    public class CodeInput
    {
        public object Value { get; private set; }
        public string Description { get; set; }
        public int Index { get; set; }

        public CodeInput(string value, string description, int index)
        {
            Value = value;
            Description = description;
            Index = index;
        }
        public CodeInput(CodeBase value, string description, int index)
        {
            Value = value;
            Description = description;
            Index = index;
        }

        public void SetValue(string value) => Value = value;
        public void SetValue(CodeBase value) => Value = value;
    }
}
