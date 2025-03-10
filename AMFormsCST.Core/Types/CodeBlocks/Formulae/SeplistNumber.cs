using AMFormsCST.Core.Types.CodeBlocks.Functions;

namespace AMFormsCST.Core.Types.CodeBlocks.Formulae
{
    public class SeplistNumber : CodeBase
    {
        public SeplistNumber()
        {

            Name = "Seplist Number";
            Description = "Allow Numeric fields to be used in seplist.";
            AddInput("Numeric Field");
            AddInput("Decimal Places");
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            return new IfCode()
                .SetInputValue(0, (GetInput(0) as string ?? string.Empty) + " != 0")
                .SetInputValue(1, new RoundCode()
                    .SetInputValue(0, GetInput(0) as string ?? string.Empty)
                    .SetInputValue(1, GetInput(1) as string ?? string.Empty))
                .SetInputValue(2, "''");
        }
    }
}
