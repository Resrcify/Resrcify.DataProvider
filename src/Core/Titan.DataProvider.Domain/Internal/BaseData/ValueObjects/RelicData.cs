using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects
{
    public sealed class RelicData : ValueObject
    {
        private readonly Dictionary<string, long> _gms = new();
        public IReadOnlyDictionary<string, long> Gms => _gms;
        private readonly Dictionary<long, long> _stats = new();
        public IReadOnlyDictionary<long, long> Stats => _stats;
        private RelicData(Dictionary<string, long> gms, Dictionary<long, long> stats)
        {
            _gms = gms;
            _stats = stats;
        }
        public static Result<RelicData> Create(Dictionary<string, long> gms, Dictionary<long, long> stats)
        {
            return new RelicData(gms, stats);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Gms;
            yield return Stats;
        }
    }
}