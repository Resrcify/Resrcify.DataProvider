using System.Collections.Generic;
using System.Linq;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.ModeSetData;

public sealed class ModSetData : ValueObject
{
    public long Id { get; private set; }
    public long Count { get; private set; }
    public long Value { get; private set; }
    public long Max { get; private set; }
    private ModSetData(long id, long count, long value, long max)
    {
        Id = id;
        Count = count;
        Value = value;
        Max = max;
    }

    public static Result<ModSetData> Create(StatModSetDefinition value)
        => new ModSetData(
            (int)value.CompleteBonus!.Stat!.UnitStatId,
            value.SetCount,
            value.CompleteBonus.Stat.UnscaledDecimalValue,
            value.MaxLevelBonus!.Stat!.UnscaledDecimalValue);
    public static Result<ModSetData> Create(
        long id,
        long count,
        long value,
        long max)
        => new ModSetData(
            id,
            count,
            value,
            max);
    public static Result<Dictionary<string, ModSetData>> Create(GameDataResponse data)
    {
        var modSet = new Dictionary<string, ModSetData>();
        foreach (var item in data.StatModSets.OrderBy(s => s.Id!.Length).ThenBy(s => s.Id))
        {
            var mod = Create(item);
            modSet.Add(item.Id!, mod.Value);
        }
        return modSet;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Count;
        yield return Value;
        yield return Max;
    }
}