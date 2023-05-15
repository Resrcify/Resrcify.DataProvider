using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects
{
    public sealed class GearLevel : ValueObject
    {
        public IReadOnlyList<string> Gear => _gear;
        private readonly List<string> _gear = new();
        public IReadOnlyDictionary<long, long> Stats => _stats;
        private readonly Dictionary<long, long> _stats = new();

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
}