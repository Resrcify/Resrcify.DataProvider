using System.Collections.Generic;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

public sealed class GearLevel : ValueObject
{
    public IReadOnlyList<string> Gear => _gear;
    private readonly List<string> _gear = [];
    public IReadOnlyDictionary<long, long> Stats => _stats;
    private readonly Dictionary<long, long> _stats = [];

    private GearLevel(List<string> gear, Dictionary<long, long> stats)
    {
        _gear = gear;
        _stats = stats;
    }

    public static Result<GearLevel> Create(List<string> gear, Dictionary<long, long> stats)
    {
        return new GearLevel(gear, stats);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Gear;
        yield return Stats;
    }
}