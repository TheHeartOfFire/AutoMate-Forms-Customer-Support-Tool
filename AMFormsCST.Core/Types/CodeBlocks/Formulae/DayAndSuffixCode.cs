using AMFormsCST.Core.Types.CodeBlocks.Functions;

namespace AMFormsCST.Core.Types.CodeBlocks.Formulae
{
    public class DayAndSuffixCode : CodeBase
    {
        public DayAndSuffixCode()
        {
            Name = "Day and Suffix";
            Description = "Returns the day of the month with the suffix. 11 = 11th, 12 = 12th, 13 = 13th, 21 = 21st, 22 = 22nd, etc.";
            AddInput("Numeric Day");
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            return new NumToTextCode()
                       .SetInputValue(0, GetInput(0) as string ?? string.Empty)
                       .SetInputValue(1, "0") +
                   " + " +
                   new CaseCode()
                .AddExtraInputs(1)
                .SetInputValue(0, GetInput(0) as string ?? string.Empty)
                .SetInputValue(1, "11")
                .SetInputValue(2, "\'th\'")
                .SetInputValue(3, "12")
                .SetInputValue(4, "\'th\'")
                .SetInputValue(5, "13")
                .SetInputValue(6, "\'th\'")
                .SetInputValue(7, new CaseCode()
                    .AddExtraInputs(1)
                    .SetInputValue(0, $"{GetInput(0) as string ?? string.Empty} % 10")
                    .SetInputValue(1, "1")
                    .SetInputValue(2, "\'st\'")
                    .SetInputValue(3, "2")
                    .SetInputValue(4, "\'nd\'")
                    .SetInputValue(5, "3")
                    .SetInputValue(6, "\'rd\'")
                    .SetInputValue(7, "\'th\'"));
        }
    }
}
