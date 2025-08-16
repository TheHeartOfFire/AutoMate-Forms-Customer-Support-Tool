using AMFormsCST.Core.Interfaces.CodeBlocks;
using System.Text;

namespace AMFormsCST.Core.Types.CodeBlocks
{
    public abstract class CodeBase : ICodeBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Prefix { get; set; }
        public List<CodeInput> Inputs { get; } = [];

        public bool HasNoInputs() => Inputs.Count == 0;

        public CodeBase AddInput(CodeInput input)
        {
            if (Inputs.Exists(x => x.Index == input.Index))
                Inputs[Inputs.IndexOf(Inputs.Find(x => x.Index == input.Index)!)] = input;
            else
                Inputs.Add(input);
            Inputs.Sort();

            return this;
        }

        public CodeBase AddInput(string description)
        {
            Inputs.Add(new CodeInput(string.Empty, description, Inputs.Count));
            Inputs.Sort();
            return this;
        }
        public CodeBase AddInput(int index, string description)
        {

            foreach (var input in Inputs.Where(input => input.Index >= index))
                input.Index++;

            Inputs.Add(new CodeInput(string.Empty, description, index));
            Inputs.Sort();

            return this;
        }
        public CodeBase RemoveInput(int index)
        {

            Inputs.RemoveAt(index);

            foreach (var input in Inputs.Where(input => input.Index >= index))
                input.Index--;

            return this;
        }
        public CodeBase AddInput(string value, string description)
        {
            Inputs.Add(new CodeInput(value, description, Inputs.Count));
            return this;
        }
        public CodeBase AddInput(CodeBase value, string description)
        {
            Inputs.Add(new CodeInput(value, description, Inputs.Count));
            return this;
        }

        public CodeBase SetInputValue(int index, string value)
        {
            Inputs[index].SetValue(value);
            return this;
        }

        public CodeBase SetInputValue(int index, CodeBase value)
        {
            Inputs[index].SetValue(value);
            return this;
        }
        public CodeBase SetInputDescription(int index, string value)
        {
            Inputs[index].Description = value;
            return this;
        }

        public CodeBase SetInputs(List<string> inputs)
        {
            if (inputs.Count != Inputs.Count)
                throw new ArgumentException("Inputs count does not match");

            foreach (var input in Inputs)
                input.SetValue(inputs[input.Index]);

            return this;
        }

        public List<CodeInput> GetInputs() => Inputs;

        public int InputCount() => Inputs.Count;


        public virtual object GetInput(int idx) => Inputs[idx].Value;
        public virtual object GetDescription(int idx) => Inputs[idx].Description;

        public virtual string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            StringBuilder output = new();
            output.Append(Prefix);
            output.Append("( ");

            var step = 0;

            foreach (var input in Inputs)
            {
                output.Append(input.Value is CodeBase @base ? @base.GetCode() : (string)input.Value == string.Empty ? input.Description : input.Value);

                if (step != Inputs.Count - 1)
                    output.Append(", ");
                step++;
            }

            output.Append(" )");
            return output.ToString().Replace("( (", "((").Replace(") )", "))");
        }

        public static implicit operator string(CodeBase @base) => @base.GetCode();
        public static string operator +(CodeBase a, CodeBase b) => a.GetCode() + b.GetCode();
        public static string operator +(string a, CodeBase b) => a + b.GetCode();
        public static string operator +(CodeBase a, string b) => a.GetCode() + b;

    }
}
