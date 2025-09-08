using AMFormsCST.Core.Types.CodeBlocks.Functions;

namespace AMFormsCST.Core.Types.CodeBlocks.Formulae
{
    public class DateConversionCode : CodeBase
    {
        public DateConversionCode()
        {
            Name = "Date Conversion";
            Description = "Converts a date in the format of MM/DD/YYYY to a date in the format of MM/DD/YY.";
            AddInput("Date Field");
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            return new SeplistCode()
                .AddExtraInputs(1)
                .SetInputValue(0, "'/'")
                .SetInputValue(1, new MonthCode()
                    .SetInputValue(0, GetInput(0) as string ?? string.Empty))
                .SetInputValue(2, new DayCode()
                    .SetInputValue(0, GetInput(0) as string ?? string.Empty))
                .SetInputValue(3, new YearCode()
                    .SetInputValue(0, GetInput(0) as string ?? string.Empty) + " % 100");
        }
    }
}
