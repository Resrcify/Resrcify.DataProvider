using System;
using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Internal.BaseData.Entities;
using Titan.DataProvider.Domain.Internal.BaseData.ValueObjects;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData
{
    public sealed class BaseData : AggregateRoot
    {
        private readonly Dictionary<string, GearData> _gear = new();
        public IReadOnlyDictionary<string, GearData> Gear => _gear;
        private readonly Dictionary<string, ModSetData> _modSet = new();
        public IReadOnlyDictionary<string, ModSetData> ModSet => _modSet;
        public CrTable CrTable { get; private set; }
        public GpTable GpTable { get; private set; }
        private BaseData(
            Guid id,
            Dictionary<string, GearData> gear,
            Dictionary<string, ModSetData> modSet,
            CrTable crTable,
            GpTable gpTable
        ) : base(id)
        {
            _gear = gear;
            _modSet = modSet;
            CrTable = crTable;
            GpTable = gpTable;
        }
        public static Result<BaseData> Create(GameDataResponse data)
        {
            var gearData = CreateGearData(data);
            var modSetData = CreateModSetData(data);
            var crTable = CrTable.Create(data);
            var crTableData = crTable.Value;
            var gpTable = GpTable.Create(
                data,
                crTableData.CrewRarityCr,
                crTableData.GearLevelCr,
                crTableData.ShipRarityFactor,
                crTableData.UnitLevelCr,
                crTableData.AbilityLevelCr
            );

            // TODO: FIX ERROR RESPONSES LATER
            return new BaseData(
                Guid.NewGuid(),
                gearData.Value,
                modSetData.Value,
                crTable.Value,
                gpTable.Value
            );
        }

        private static Result<Dictionary<string, GearData>> CreateGearData(GameDataResponse data)
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
                gearData.Add(item!.Id!, GearData.Create(item).Value);
            }
            return gearData;
        }

        private static Result<Dictionary<string, ModSetData>> CreateModSetData(GameDataResponse data)
        {
            var modSet = new Dictionary<string, ModSetData>();
            foreach (var item in data.StatModSet.OrderBy(s => s.Id!.Length).ThenBy(s => s.Id))
            {
                var mod = ModSetData.Create(item);
                modSet.Add(item.Id!, mod.Value);
            }
            return modSet;
        }
    }
}