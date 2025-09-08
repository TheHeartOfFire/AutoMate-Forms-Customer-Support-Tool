using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class MonthNameCode : CodeBase
    {
        public MonthNameCode()
        {
            Name = "Month Names";
            Description = "Get the name of the specified month. 1 = January, 2 = February, etc.";
            AddInput("Date Field");
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            var input = GetInput(0);
            string inputValue = input is CodeBase cb ? cb.GetCode() : input as string ?? string.Empty;

            return new CaseCode()
                .AddExtraInputs(10)
                .SetInputValue(0, inputValue)
                .SetInputDescription(0, string.Empty)
                .SetInputValue(1, "1")
                .SetInputValue(2, "'JANUARY'")
                .SetInputValue(3, "2")
                .SetInputValue(4, "'FEBRUARY'")
                .SetInputValue(5, "3")
                .SetInputValue(6, "'MARCH'")
                .SetInputValue(7, "4")
                .SetInputValue(8, "'APRIL'")
                .SetInputValue(9, "5")
                .SetInputValue(10, "'MAY'")
                .SetInputValue(11, "6")
                .SetInputValue(12, "'JUNE'")
                .SetInputValue(13, "7")
                .SetInputValue(14, "'JULY'")
                .SetInputValue(15, "8")
                .SetInputValue(16, "'AUGUST'")
                .SetInputValue(17, "9")
                .SetInputValue(18, "'SEPTEMBER'")
                .SetInputValue(19, "10")
                .SetInputValue(20, "'OCTOBER'")
                .SetInputValue(21, "11")
                .SetInputValue(22, "'NOVEMBER'")
                .SetInputValue(23, "12")
                .SetInputValue(24, "'DECEMBER'")
                .SetInputValue(25, "'Invalid Month'");
        }
    }
}
