using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.ExpandedDatacron.ValueObjects;
public sealed class StatTier : ValueObject
{
    public UnitStat UnitStat { get; private set; }
    public int Tier { get; private set; }
    public UnitTier RequiredUnitTier { get; private set; }
    public RelicTier RequiredRelicTier { get; private set; }
    public string Name { get; private set; }
    public double Value { get; private set; }


    private StatTier(UnitStat unitStat, int tier, UnitTier requiredUnitTier, RelicTier requiredRelicTier, string name, double value)
    {
        UnitStat = unitStat;
        Tier = tier;
        RequiredUnitTier = requiredUnitTier;
        RequiredRelicTier = requiredRelicTier;
        Name = name;
        Value = value;
    }

    public static Result<StatTier> Create(int tier, DatacronAffix playerAffix)
    {
        return new StatTier(playerAffix.StatType, tier, playerAffix.RequiredUnitTier, playerAffix.RequiredRelicTier, GetInGameName((int)playerAffix.StatType), playerAffix.StatValue / 1e8);
    }

    private static string GetInGameName(int enumValue)
       => enumValue switch
       {
           1 => "Health",
           2 => "Strength",
           3 => "Agility",
           4 => "Tactics",
           5 => "Speed",
           6 => "Physical Damage",
           7 => "Special Damage",
           8 => "Armor",
           9 => "Resistance",
           10 => "Armor Penetration",
           11 => "Resistance Penetration",
           12 => "Dodge Chance",
           13 => "Deflection Chance",
           14 => "Physical Critical Chance",
           15 => "Special Critical Chance",
           16 => "Critical Damage",
           17 => "Potency",
           18 => "Tenacity",
           19 => "Dodge",
           20 => "Deflection",
           21 => "Physical Critical Chance",
           22 => "Special Critical Chance",
           23 => "Armor",
           24 => "Resistance",
           25 => "Armor Penetration",
           26 => "Resistance Penetration",
           27 => "Health Steal",
           28 => "Protection",
           29 => "Protection Ignore",
           30 => "Health Regeneration",
           31 => "Physical Damage",
           32 => "Special Damage",
           33 => "Physical Accuracy",
           34 => "Special Accuracy",
           35 => "Physical Critical Avoidance",
           36 => "Special Critical Avoidance",
           37 => "Physical Accuracy",
           38 => "Special Accuracy",
           39 => "Physical Critical Avoidance",
           40 => "Special Critical Avoidance",
           41 => "Offense",
           42 => "Defense",
           43 => "Defense Penetration",
           44 => "Evasion",
           45 => "Critical Chance",
           46 => "Accuracy",
           47 => "Critical Avoidance",
           48 => "Offense",
           49 => "Defense",
           50 => "Defense Penetration",
           51 => "Evasion",
           52 => "Accuracy",
           53 => "Critical Chance",
           54 => "Critical Avoidance",
           55 => "Health",
           56 => "Protection",
           57 => "Speed",
           58 => "Counter Attack",
           59 => "UnitStat_Taunt",
           61 => "Mastery",
           _ => "None"
       };
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return UnitStat;
        yield return Tier;
        yield return RequiredUnitTier;
        yield return RequiredRelicTier;
        yield return Name;
        yield return Value;
    }
}