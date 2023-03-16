using System;
using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.Entities
{
    public sealed class GearData : ValueObject
    {
        public IReadOnlyDictionary<long, long> Stat => _stat;
        private readonly Dictionary<long, long> _stat = new();
        private GearData(Dictionary<long, long> stat)
        {
            _stat = stat;
        }

        public static Result<GearData> Create(EquipmentDef data)
        {
            return new GearData(ConvertToGearData(data));
        }
        //Maps all gear data to { gearData:{ ... } }
        private static Dictionary<long, long> ConvertToGearData(EquipmentDef data)
        {
            var stat = new Dictionary<long, long>();
            if (data?.EquipmentStat?.Stat is not null && data?.EquipmentStat?.Stat.Count > 0)
            {
                foreach (var subItem in data.EquipmentStat.Stat.OrderBy(s => s.UnitStatId))
                {
                    long key = (int)subItem.UnitStatId;
                    stat.Add(key, subItem.UnscaledDecimalValue);
                }
            }
            return stat;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Stat;
        }
    }
}
