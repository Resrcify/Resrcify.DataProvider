using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects
{
    public sealed class GearData : ValueObject
    {
        public IReadOnlyDictionary<long, long> Stats => _stats;
        private readonly Dictionary<long, long> _stats = new();
        private GearData(Dictionary<long, long> stats)
        {
            _stats = stats;
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

        public static Result<Dictionary<string, GearData>> Create(GameDataResponse data)
        {
            var gearData = new Dictionary<string, GearData>();
            foreach (var item in data.Equipment.OrderBy(s => s.Id!.Length).ThenBy(s => s.Id))
            {
                var stat = new Dictionary<long, long>();
                if (item?.EquipmentStat?.Stat is not null && item?.EquipmentStat?.Stat.Count > 0)
                {
                    foreach (var subItem in item.EquipmentStat.Stat.OrderBy(s => s.UnitStatId))
                    {
                        long key = (int)subItem.UnitStatId;
                        stat.Add(key, subItem.UnscaledDecimalValue);
                    }
                }
                gearData.Add(item!.Id!, Create(item).Value);
            }
            return gearData;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Stats;
        }
    }
}
