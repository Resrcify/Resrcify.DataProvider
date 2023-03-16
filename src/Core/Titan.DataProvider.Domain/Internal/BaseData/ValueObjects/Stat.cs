using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class Stat : ValueObject
    {
        public IReadOnlyDictionary<long, long> Data => _data;
        private readonly Dictionary<long, long> _data = new();

        private Stat(Dictionary<long, long> data)
        {
            _data = data;
        }

        public static Result<Stat> Create(EquipmentDef value)
        {
            var stat = new Dictionary<long, long>();
            if (value?.EquipmentStat?.Stat is not null && value?.EquipmentStat?.Stat.Count > 0)
            {
                foreach (var subItem in value.EquipmentStat.Stat.OrderBy(s => s.UnitStatId))
                {
                    long key = (int)subItem.UnitStatId;
                    stat.Add(key, subItem.UnscaledDecimalValue);
                }
            }
            return new Stat(stat);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Data;
        }
    }
}