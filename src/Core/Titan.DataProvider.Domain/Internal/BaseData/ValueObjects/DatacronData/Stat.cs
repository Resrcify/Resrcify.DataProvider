using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed class Stat : ValueObject
{
    public int Id { get; private set; }
    public string NameKey { get; private set; }
    public string IconKey { get; private set; }

    private Stat(int id, string nameKey, string iconKey)
    {
        Id = id;
        NameKey = nameKey;
        IconKey = iconKey;


    }
    public static Result<Stat> Create(int id, string nameKey, string iconKey)
    {
        return new Stat(id, nameKey, iconKey);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return NameKey;
        yield return IconKey;
    }
}