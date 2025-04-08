using System.Collections.Generic;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed class Target : ValueObject
{
    public string Id { get; private set; }
    public string NameKey { get; private set; }
    public string DescKey { get; private set; }
    public string IconKey { get; private set; }
    public Unit? Unit { get; private set; }
    private Target(string id, string nameKey, string descKey, string iconKey, Unit? unit)
    {
        Id = id;
        NameKey = nameKey;
        DescKey = descKey;
        IconKey = iconKey;
        Unit = unit;
    }
    public static Result<Target> Create(string id, string nameKey, string descKey, string iconKey, Unit? unit = null)
    {
        return new Target(id, nameKey, descKey, iconKey, unit);
    }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return NameKey;
        yield return DescKey;
        yield return IconKey;
        yield return Unit!;
    }
}