using System.Collections.Generic;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed class Unit : ValueObject
{
    public string BaseId { get; private set; }
    public string NameKey { get; private set; }
    public int CombatType { get; private set; }
    private Unit(string baseId, string nameKey, int combatType)
    {
        BaseId = baseId;
        NameKey = nameKey;
        CombatType = combatType;

    }
    public static Result<Unit> Create(string baseId, string nameKey, int combatType)
    {
        return new Unit(baseId, nameKey, combatType);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return BaseId;
        yield return NameKey;
        yield return CombatType;
    }
}