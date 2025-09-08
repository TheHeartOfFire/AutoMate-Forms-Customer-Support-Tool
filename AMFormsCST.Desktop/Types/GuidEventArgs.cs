namespace AMFormsCST.Desktop.Types
{
    public class GuidEventArgs(Guid value) : System.EventArgs
    {
        public Guid Value { get; } = value;
    }
}
