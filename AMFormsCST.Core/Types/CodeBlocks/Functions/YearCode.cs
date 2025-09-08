namespace AMFormsCST.Core.Types.CodeBlocks.Functions
{
    public class YearCode : CodeBase
    {
        public YearCode()
        {
            Name = "Year";
            Prefix = "YEAR";
            Description = "Extract the numeric year from a date";
            AddInput("Date Field");
        }
    }
}
