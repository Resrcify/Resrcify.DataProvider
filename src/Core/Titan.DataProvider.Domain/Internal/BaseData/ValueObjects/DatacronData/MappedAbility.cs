using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed class MappedAbility : ValueObject
{
    public string NameKey { get; private set; }
    public string DescKey { get; private set; }
    public string IconKey { get; private set; }
    private readonly Dictionary<string, Target> _targets = new();
    public IReadOnlyDictionary<string, Target> Targets => _targets;
    private MappedAbility(string nameKey, string descKey, string iconKey)
    {
        DescKey = descKey;
        NameKey = nameKey;
        IconKey = iconKey;

    }
    public static Result<MappedAbility> Create(string nameKey, string descKey, string iconKey)
    {
        return new MappedAbility(nameKey, descKey, iconKey);
    }
    public Result AddTarget(string key, Target target)
    {
        _targets.TryAdd(key, target);
        return Result.Success();
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return NameKey;
        yield return DescKey;
        yield return IconKey;
        yield return Targets;
    }
}