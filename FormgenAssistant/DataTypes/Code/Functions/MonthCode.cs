namespace FormgenAssistant.DataTypes.Code.Functions
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

        public override string GetCode()
        {
            // Use the input value or description as appropriate
            var input = GetInput(0);
            string inputValue = input as string ?? (input is CodeBase cb ? cb.GetCode() : Inputs[0].Description);
            return $"MONTH( {inputValue} )";
        }
    }
}
