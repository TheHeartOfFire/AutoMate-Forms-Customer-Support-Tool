namespace AMFormsCST.Core.Interfaces.Notebook;

public interface ITestDeal : INotable<ITestDeal>
{
    string DealNumber { get; set; }
    string Purpose { get; set; }
}