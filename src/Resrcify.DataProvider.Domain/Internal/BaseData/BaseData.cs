using System;
using System.Collections.Generic;
using System.Linq;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.CrTable;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GearData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GpTable;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.ModeSetData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.RelicData;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Domain.Internal.BaseData;

public sealed class BaseData : AggregateRoot<Guid>
{
    private readonly Dictionary<string, GearData> _gear = [];
    public IReadOnlyDictionary<string, GearData> Gear => _gear;
    private readonly Dictionary<string, ModSetData> _modSets = [];
    public IReadOnlyDictionary<string, ModSetData> ModSets => _modSets;
    public CrTable CrTable { get; private set; }
    public GpTable GpTable { get; private set; }
    private readonly Dictionary<string, RelicData> _relics = [];
    public IReadOnlyDictionary<string, RelicData> Relics => _relics;
    private readonly Dictionary<string, UnitData> _units = [];
    public IReadOnlyDictionary<string, UnitData> Units => _units;
    public IReadOnlyDictionary<string, DatacronData> Datacrons => _datacrons;
    private readonly Dictionary<string, DatacronData> _datacrons = [];

    private BaseData(
        Guid id,
        Dictionary<string, GearData> _gear,
        Dictionary<string, ModSetData> _modSets,
        CrTable crTable,
        GpTable gpTable,
        Dictionary<string, RelicData> _relics,
        Dictionary<string, UnitData> _units,
        Dictionary<string, DatacronData> _datacrons)
        : base(id)
    {
        this._gear = _gear;
        this._modSets = _modSets;
        CrTable = crTable;
        GpTable = gpTable;
        this._relics = _relics;
        this._units = _units;
        this._datacrons = _datacrons;
    }
    public static BaseData Create(
        Guid id,
        Dictionary<string, GearData> gear,
        Dictionary<string, ModSetData> modSets,
        CrTable crTable,
        GpTable gpTable,
        Dictionary<string, RelicData> relics,
        Dictionary<string, UnitData> units,
        Dictionary<string, DatacronData> datacrons)
    {
        return new BaseData(
            id,
            gear,
            modSets,
            crTable,
            gpTable,
            relics,
            units,
            datacrons);
    }
    public static Result<BaseData> Create(GameDataResponse data, List<string> localization)
    {
        var gearData = GearData.Create(data);
        var modSetData = ModSetData.Create(data);
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
        var statsTable = FetchStatsTable(data);
        var relicData = RelicData.Create(data, statsTable);

        var local = GetLocalizationDictionary(localization);
        var skills = Skill.Create(data, local);
        var growthModifiers = CreateGrowthModifiers(data, statsTable);
        var unitData = UnitData.Create(data, local, skills.Value, growthModifiers, statsTable);
        var datacronData = DatacronData.Create(data, local);

        // TODO: FIX ERROR RESPONSES LATER
        return new BaseData(
            Guid.NewGuid(),
            gearData.Value,
            modSetData.Value,
            crTable.Value,
            gpTable.Value,
            relicData.Value,
            unitData,
            datacronData.Value
        );
    }

    private static Dictionary<string, Dictionary<string, long>> FetchStatsTable(GameDataResponse data)
    {
        var statsTable = new Dictionary<string, Dictionary<string, long>>();
        foreach (var table in data.StatProgressions)
        {
            if (table.Id is not null &&
                table.Id.StartsWith("stattable_"))
            {
                var statsLine = new Dictionary<string, long>();
                foreach (var stat in table.Stat!.Stats.OrderBy(s => (int)s.UnitStatId))
                {
                    var id = (int)stat.UnitStatId;
                    statsLine[id.ToString()] = stat.UnscaledDecimalValue;
                }
                statsTable[table.Id] = statsLine;
            }
        }
        return statsTable;
    }

    private static Dictionary<string, string> GetLocalizationDictionary(List<string> localization)
    {
        var tmp = new Dictionary<string, string>();
        foreach (string line in localization)
        {
            var split = line.Split("|");
            if (split.Length > 1)
                tmp[split[0]] = split[1];
        }
        return tmp;
    }

    private static Dictionary<string, Dictionary<string, Dictionary<string, long>>> CreateGrowthModifiers(GameDataResponse data, Dictionary<string, Dictionary<string, long>> statsTable)
    {
        var ul = new Dictionary<string, Dictionary<string, Dictionary<string, long>>>();
        foreach (var unit in data.Units.Where(u => u.Obtainable && u.ObtainableTime == 0).OrderBy(s => (int)s.Rarity))
        {
            var rarity = (int)unit.Rarity;
            var baseId = unit.BaseId ?? string.Empty;
            var statProgressionId = unit.StatProgressionId;

            if (!ul.ContainsKey(baseId))
                ul[baseId] = [];

            ul[baseId][rarity.ToString()] = statsTable[statProgressionId ?? string.Empty];
        }
        return ul;
    }
}