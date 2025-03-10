using AMFormsCST.Core.Interfaces.CodeBlocks;
using AMFormsCST.Core.Types.CodeBlocks.Formulae;
using AMFormsCST.Core.Types.CodeBlocks.Functions;

namespace AMFormsCST.Core.Utils;
public class CodeBlocks : ICodeBlocks
{
    public IList<ICodeBase> CustomBlocks { get; set; } = [];
    private readonly IList<ICodeBase> _defaultBlocks = [];
    public CodeBlocks()
    {
        _defaultBlocks.Add(new CityStateZIPCode());
        _defaultBlocks.Add(new DateConversionCode());
        _defaultBlocks.Add(new DayAndSuffixCode());
        _defaultBlocks.Add(new MonthNameCode());
        _defaultBlocks.Add(new NumToTextCode());
        _defaultBlocks.Add(new SeplistNumber());
        _defaultBlocks.Add(new DmvCalculationCode());
        _defaultBlocks.Add(new FuelDropdownDefaultCode());
        _defaultBlocks.Add(new CaseCode());
        _defaultBlocks.Add(new IfCode());
        _defaultBlocks.Add(new SeplistCode());
    }

    public IReadOnlyCollection<ICodeBase> GetBlocks() => [.. _defaultBlocks, .. CustomBlocks];



}
