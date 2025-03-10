using AMFormsCST.Core.Interfaces.FormgenUtils;

namespace AMFormsCST.Core.Types.CodeBlocks.Functions
{
    public class CaseCode : CodeBase, IExtendableCode
    {
        public int DefaultArgCount { get; }
        public int ArgIncrement { get; }

        public CaseCode()
        {
            Name = "Case Statement";
            Prefix = "CASE";
            Description = "Compare the comparison value to the case values, " +
                          "return the corresponding result of any matches, " +
                          "otherwise return the default value. " +
                          "Identical to multiple nested If Statements.";
            AddInput("Comparison");
            AddInput("Case");
            AddInput("Result");
            AddInput("Case");
            AddInput("Result");
            AddInput("Default");

            DefaultArgCount = 6;
            ArgIncrement = 2;
        }


        public CodeBase AddExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
            {
                AddInput(InputCount() - 1, "Case");
                AddInput(InputCount() - 1, "Result");
            }

            return this;
        }

        public CodeBase RemoveExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (InputCount() <= DefaultArgCount) return this;

                RemoveInput(InputCount() - 2);
                RemoveInput(InputCount() - 2);
            }

            return this;
        }
    }
}
