using System.Collections.Generic;
using System.Linq;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.RelicData;

public sealed class RelicData : ValueObject
{
    private readonly Dictionary<string, long> _gms = [];
    public IReadOnlyDictionary<string, long> Gms => _gms;
    private readonly Dictionary<long, long> _stats = [];
    public IReadOnlyDictionary<long, long> Stats => _stats;
    private RelicData(Dictionary<string, long> gms, Dictionary<long, long> stats)
    {
        _gms = gms;
        _stats = stats;
    }
    public static Result<RelicData> Create(Dictionary<string, long> gms, Dictionary<long, long> stats)
        => new RelicData(gms, stats);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Gms;
        yield return Stats;
    }
    public static Result<Dictionary<string, RelicData>> Create(GameDataResponse data, Dictionary<string, Dictionary<string, long>> statsTable)
    {
        var relicData = new Dictionary<string, RelicData>();
        foreach (var relic in data.RelicTierDefinitions.OrderBy(s => s.Id!.Length).ThenBy(s => s.Id))
        {
            var stats = new Dictionary<long, long>();
            foreach (var stat in relic.Stat!.Stats.OrderBy(s => (int)s.UnitStatId))
            {
                stats[(int)stat.UnitStatId] = stat.UnscaledDecimalValue;
            }
            var statsTableForRelics = statsTable[relic.RelicStatTable!.ToString()];
            var relicDataItem = Create(statsTableForRelics, stats);
            relicData.Add(relic.Id!, relicDataItem.Value);
        }
        return relicData;
    }
}