namespace AMFormsCST.Core.Types.CodeBlocks.Formulae
{
    public class DmvCalculationCode : CodeBase
    {
        public DmvCalculationCode()
        {
            Name = "DMV Calculation";
            Description = "The sum of taxable costs in the deal. " +
                          "Selling Price + Taxed Accessory Total Price + Taxed Fees Total Amount + Taxed GAP Amount + Taxed Service Contracts Total Amount - Non-Taxed Rebates Total Amount - Total Non-taxable Trade Allowance Amount";
        }

        public override string GetCode()
        {
            return "F3 + F1953 + F1979 + F1997 + F1983 - F2008 - F2493";
        }
    }
}
