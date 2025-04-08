using System.Collections.Generic;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed class Faction : ValueObject
{
    public string Id { get; private set; }
    public string NameKey { get; private set; }
    public bool Visible { get; private set; }
    public IReadOnlyList<Unit> Units => _units;
    private readonly List<Unit> _units = [];
    private Faction(string id, string nameKey, bool visible)
    {
        Id = id;
        NameKey = nameKey;
        Visible = visible;

    }
    public static Result<Faction> Create(string id, string nameKey, bool visible)
    {
        return new Faction(id, nameKey, visible);
    }
    public Result AddUnit(Unit unit)
    {
        if (!_units.Contains(unit))
            _units.Add(unit);
        return Result.Success();
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return NameKey;
        yield return Visible;
        yield return Units;
    }
}