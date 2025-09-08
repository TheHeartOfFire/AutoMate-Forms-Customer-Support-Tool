namespace AMFormsCST.Core.Interfaces.BestPractices;
public interface IFormNameBestPractice
{
    string Generate();
    IFormModel Model { get; set; }
}
