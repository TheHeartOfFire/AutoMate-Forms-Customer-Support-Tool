using AMFormsCST.Core.Types.CodeBlocks.Functions;

namespace AMFormsCST.Core.Types.CodeBlocks.Formulae
{
    public class CityStateZIPCode : CodeBase
    {
        public CityStateZIPCode()
        {
            Name = "City, State ZIP";
            Description = "Nicely format a city, state, and ZIP code.";
            AddInput("City");
            AddInput("State");
            AddInput("ZIP");
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            return new SeplistCode()
                .SetInputValue(0, "\' \'")
                .SetInputValue(1, new SeplistCode()
                    .SetInputValue(0, "\', \'")
                    .SetInputValue(1, GetInput(0) is CodeBase ? 
                    GetInput(0) as CodeBase ?? string.Empty : 
                    GetInput(0) as string ?? string.Empty)
                    .SetInputValue(2, GetInput(1) is CodeBase ? 
                    GetInput(1) as CodeBase ?? string.Empty : 
                    GetInput(1) as string ?? string.Empty))
                .SetInputValue(2, GetInput(2) is CodeBase ?
                    GetInput(2) as CodeBase ?? string.Empty :
                    GetInput(2) as string ?? string.Empty);
        }
    }
}
