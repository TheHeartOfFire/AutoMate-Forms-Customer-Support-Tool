namespace AMFormsCST.Core.Types.CodeBlocks.Functions
{
    public class MonthCode : CodeBase
    {
        public MonthCode()
        {
            Name = "Month";
            Prefix = "MONTH";
            Description = "Extract the numeric month from a date";
            AddInput("Date Field");
        }
    }
}
