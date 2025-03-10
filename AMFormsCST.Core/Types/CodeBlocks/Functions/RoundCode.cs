namespace AMFormsCST.Core.Types.CodeBlocks.Functions
{
    public class RoundCode : CodeBase
    {
        public RoundCode()
        {
            Name = "Round";
            Prefix = "ROUND";
            Description = "Round a number to a specified number of decimal places. Uses nearest rounding. i.e. 0.5 rounds to 1.0, 0.4 rounds to 0.0";
            AddInput("Number");
            AddInput("Decimal Places");
        }
    }
}
