using AMFormsCST.Core.Types.CodeBlocks.Functions;

namespace AMFormsCST.Core.Types.CodeBlocks.Formulae
{
    public class NumToTextCode : CodeBase
    {
        public NumToTextCode()
        {
            Name = "Number to Text";
            Description = "Converts a number to text. " +
                          "This is needed because just using the NUM() function produces strange results in FormGen. " +
                          "Can also be used to left justify numbers";
            AddInput("Number");
            AddInput("Decimal Places");
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            return new TextCode()
                .SetInputValue(0, new RoundCode()
                    .SetInputValue(0, GetInput(0) as string ?? string.Empty)
                    .SetInputValue(1, GetInput(1) as string ?? string.Empty));
        }
    }
}
