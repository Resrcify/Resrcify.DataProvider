using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Errors;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Enums;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;

public sealed class Mod : ValueObject
{
    private const int MAX_LEVEL = 15;
    public ModSlot Slot { get; private set; }
    public string SlotName { get; private set; }
    public ModType Type { get; private set; }
    public string TypeName { get; private set; }
    public ModRarity Rarity { get; private set; }
    public ModTier Tier { get; private set; }
    public string TierName { get; private set; }
    public int Level { get; private set; }
    public ModStat PrimaryStat { get; private set; }
    public IReadOnlyList<ModStat> SecondaryStats => _secondaryStats;
    private readonly List<ModStat> _secondaryStats = new();
    public bool IsMaxLevel { get; private set; }
    public int RerolledCount { get; private set; }

    private Mod(ModSlot modSlot, ModType modType, ModRarity modRarity, ModTier modTier, string modSlotName, string modTypeName, string modTierName, int level, ModStat primaryStat, List<ModStat> secondaryStats, bool isMaxLevel, int rerolledCount)
    {
        Slot = modSlot;
        Type = modType;
        Rarity = modRarity;
        Tier = modTier;
        SlotName = modSlotName;
        TypeName = modTypeName;
        TierName = modTierName;
        Level = level;
        PrimaryStat = primaryStat;
        _secondaryStats = secondaryStats;
        IsMaxLevel = isMaxLevel;
        RerolledCount = rerolledCount;
    }

    public static Result<Mod> Create(StatMod statMod)
    {
        if (statMod.PrimaryStat?.Stat is null)
            return Result.Failure<Mod>(DomainErrors.Mod.PrimaryStatNotFound);
        var primaryStat = ModStat.Create(statMod.PrimaryStat.Stat.UnitStatId, statMod.PrimaryStat.Stat.UnscaledDecimalValue, 0);
        if (primaryStat.IsFailure)
            return Result.Failure<Mod>(DomainErrors.ModStat.UnableToCreate);
        var secondaryStats = new List<ModStat>();
        foreach (var secondaryStat in statMod.SecondaryStat)
        {
            if (secondaryStat.Stat is null) continue;
            var modStat = ModStat.Create(secondaryStat.Stat.UnitStatId, secondaryStat.Stat.UnscaledDecimalValue, secondaryStat.StatRolls);
            if (modStat.IsFailure) continue;
            secondaryStats.Add(modStat.Value);
        }
        var isMaxLevel = statMod.Level == MAX_LEVEL;
        var defIdArray = statMod.DefinitionId!.Select(digit => int.Parse(digit.ToString())).ToArray();
        var setId = (ModType)defIdArray[0];
        var rarity = (ModRarity)defIdArray[1];
        var slot = (ModSlot)defIdArray[2];
        var setName = GetModTypeName((int)setId);
        var slotName = GetModSlotName((int)slot);
        var modTier = (ModTier)statMod.Tier;
        return new Mod(slot, setId, rarity, modTier, slotName, setName, modTier.ToString(), statMod.Level, primaryStat.Value, secondaryStats, isMaxLevel, statMod.RerolledCount);
    }

    public static Result<List<Mod>> Create(List<StatMod> statMods)
    {
        var modList = new List<Mod>();
        foreach (var mod in statMods)
        {
            var modStat = Create(mod);
            if (modStat.IsFailure) continue;
            modList.Add(modStat.Value);
        }
        return modList;
    }

    private static string GetModSlotName(int enumValue)
        => enumValue switch
        {
            1 => "Square",
            2 => "Arrow",
            3 => "Diamond",
            4 => "Triangle",
            5 => "Circle",
            6 => "Cross",
            _ => "None"
        };
    private static string GetModTypeName(int enumValue)
        => enumValue switch
        {
            1 => "Health",
            2 => "Offence",
            3 => "Defence",
            4 => "Speed",
            5 => "Critical Chance",
            6 => "Critical Damage",
            7 => "Potency",
            8 => "Tenacity",
            _ => "None"
        };

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Slot;
        yield return SlotName;
        yield return Type;
        yield return TypeName;
        yield return Rarity;
        yield return Tier;
        yield return TierName;
        yield return Level;
        yield return PrimaryStat;
        yield return SecondaryStats;
        yield return IsMaxLevel;
        yield return RerolledCount;
    }
}