using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

public sealed class Ability : ValueObject
{
    public string Id { get; private set; }
    private readonly Dictionary<string, Target> _targets = new();
    public IReadOnlyDictionary<string, Target> Targets => _targets;
    private Ability(string id, Dictionary<string, Target> targets)
    {
        Id = id;
        _targets = targets;

    }
    public static Result<Ability> Create(string id, Dictionary<string, Target>? targets = null)
    {
        if (targets is null)
            return new Ability(id, new Dictionary<string, Target>());
        return new Ability(id, targets);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Targets;
    }
}