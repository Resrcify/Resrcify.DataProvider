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
    public bool IsPercentage { get; private set; }


    private StatTier(UnitStat unitStat, int tier, UnitTier requiredUnitTier, RelicTier requiredRelicTier, string name, double value, bool isPercentage)
    {
        UnitStat = unitStat;
        Tier = tier;
        RequiredUnitTier = requiredUnitTier;
        RequiredRelicTier = requiredRelicTier;
        Name = name;
        Value = value;
        IsPercentage = isPercentage;
    }

    public static Result<StatTier> Create(int tier, DatacronAffix playerAffix)
    {
        var isPercentage = EnumIsPercentage(playerAffix.StatType);
        var statValue = playerAffix.StatValue / 1e8;
        if (isPercentage) statValue *= 100;
        return new StatTier(playerAffix.StatType, tier, playerAffix.RequiredUnitTier, playerAffix.RequiredRelicTier, GetInGameName(playerAffix.StatType), statValue, isPercentage);
    }

    private static bool EnumIsPercentage(UnitStat enumValue)
        //As a rule of thumb:
        //Additive are percentages
        //Rating are flat values but normally calculated as percentages
        //Without additions are flat values
        => enumValue switch
        {
            UnitStat.UNITSTATARMOR or //Actually not percentage, this value is converted to percentage to mimic games presentation
            UnitStat.UNITSTATSUPPRESSION or  //Actually not percentage, this value is converted to percentage to mimic games presentation
            UnitStat.UNITSTATDODGERATING or //Actually not percentage, this value is converted to percentage to mimic games presentation
            UnitStat.UNITSTATDEFLECTIONRATING or //Actually not percentage, this value is converted to percentage to mimic games presentation
            UnitStat.UNITSTATATTACKCRITICALRATING or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
            UnitStat.UNITSTATABILITYCRITICALRATING or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
            UnitStat.UNITSTATCRITICALDAMAGE or
            UnitStat.UNITSTATACCURACY or
            UnitStat.UNITSTATRESISTANCE or
            UnitStat.UNITSTATDODGEPERCENTADDITIVE or
            UnitStat.UNITSTATDEFLECTIONPERCENTADDITIVE or
            UnitStat.UNITSTATATTACKCRITICALPERCENTADDITIVE or
            UnitStat.UNITSTATABILITYCRITICALPERCENTADDITIVE or
            UnitStat.UNITSTATARMORPERCENTADDITIVE or
            UnitStat.UNITSTATSUPPRESSIONPERCENTADDITIVE or
            UnitStat.UNITSTATARMORPENETRATIONPERCENTADDITIVE or
            UnitStat.UNITSTATSUPPRESSIONPENETRATIONPERCENTADDITIVE or
            UnitStat.UNITSTATHEALTHSTEAL or
            UnitStat.UNITSTATATTACKDAMAGEPERCENTADDITIVE or
            UnitStat.UNITSTATABILITYPOWERPERCENTADDITIVE or
            UnitStat.UNITSTATDODGENEGATEPERCENTADDITIVE or
            UnitStat.UNITSTATDEFLECTIONNEGATEPERCENTADDITIVE or
            UnitStat.UNITSTATATTACKCRITICALNEGATEPERCENTADDITIVE or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
            UnitStat.UNITSTATABILITYCRITICALNEGATEPERCENTADDITIVE or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
            UnitStat.UNITSTATDODGENEGATERATING or  //Actually not percentage, this value is converted to percentage to mimic games presentation
            UnitStat.UNITSTATDEFLECTIONNEGATERATING or  //Actually not percentage, this value is converted to percentage to mimic games presentation
            UnitStat.UNITSTATATTACKCRITICALNEGATERATING or
            UnitStat.UNITSTATABILITYCRITICALNEGATERATING or
            UnitStat.UNITSTATEVASIONRATING or
            UnitStat.UNITSTATCRITICALRATING or
            UnitStat.UNITSTATEVASIONNEGATERATING or
            UnitStat.UNITSTATCRITICALNEGATERATING or
            UnitStat.UNITSTATOFFENSEPERCENTADDITIVE or
            UnitStat.UNITSTATDEFENSEPERCENTADDITIVE or
            UnitStat.UNITSTATDEFENSEPENETRATIONPERCENTADDITIVE or
            UnitStat.UNITSTATEVASIONPERCENTADDITIVE or
            UnitStat.UNITSTATEVASIONNEGATEPERCENTADDITIVE or
            UnitStat.UNITSTATCRITICALCHANCEPERCENTADDITIVE or
            UnitStat.UNITSTATCRITICALNEGATECHANCEPERCENTADDITIVE or
            UnitStat.UNITSTATMAXHEALTHPERCENTADDITIVE or
            UnitStat.UNITSTATMAXSHIELDPERCENTADDITIVE or
            UnitStat.UNITSTATSPEEDPERCENTADDITIVE or
            UnitStat.UNITSTATCOUNTERATTACKRATING or
            UnitStat.UNITSTATDEFENSEPENETRATIONTARGETPERCENTADDITIVE
            => true,
            _ => false
        };


    private static string GetInGameName(UnitStat unitStat)
        => unitStat switch
        {
            UnitStat.UNITSTATMAXHEALTH => "Health",
            UnitStat.UNITSTATSTRENGTH => "Strength",
            UnitStat.UNITSTATAGILITY => "Agility",
            UnitStat.UNITSTATINTELLIGENCE => "Tactics",
            UnitStat.UNITSTATSPEED => "Speed",
            UnitStat.UNITSTATATTACKDAMAGE => "Physical Damage",
            UnitStat.UNITSTATABILITYPOWER => "Special Damage",
            UnitStat.UNITSTATARMOR => "Armor",
            UnitStat.UNITSTATSUPPRESSION => "Resistance",
            UnitStat.UNITSTATARMORPENETRATION => "Armor Penetration",
            UnitStat.UNITSTATSUPPRESSIONPENETRATION => "Resistance Penetration",
            UnitStat.UNITSTATDODGERATING => "Dodge Chance",
            UnitStat.UNITSTATDEFLECTIONRATING => "Deflection Chance",
            UnitStat.UNITSTATATTACKCRITICALRATING => "Physical Critical Chance",
            UnitStat.UNITSTATABILITYCRITICALRATING => "Special Critical Chance",
            UnitStat.UNITSTATCRITICALDAMAGE => "Critical Damage",
            UnitStat.UNITSTATACCURACY => "Potency",
            UnitStat.UNITSTATRESISTANCE => "Tenacity",
            UnitStat.UNITSTATDODGEPERCENTADDITIVE => "Dodge",
            UnitStat.UNITSTATDEFLECTIONPERCENTADDITIVE => "Deflection",
            UnitStat.UNITSTATATTACKCRITICALPERCENTADDITIVE => "Physical Critical Chance",
            UnitStat.UNITSTATABILITYCRITICALPERCENTADDITIVE => "Special Critical Chance",
            UnitStat.UNITSTATARMORPERCENTADDITIVE => "Armor",
            UnitStat.UNITSTATSUPPRESSIONPERCENTADDITIVE => "Resistance",
            UnitStat.UNITSTATARMORPENETRATIONPERCENTADDITIVE => "Armor Penetration",
            UnitStat.UNITSTATSUPPRESSIONPENETRATIONPERCENTADDITIVE => "Resistance Penetration",
            UnitStat.UNITSTATHEALTHSTEAL => "Health Steal",
            UnitStat.UNITSTATMAXSHIELD => "Protection",
            UnitStat.UNITSTATSHIELDPENETRATION => "Protection Ignore",
            UnitStat.UNITSTATHEALTHREGEN => "Health Regeneration",
            UnitStat.UNITSTATATTACKDAMAGEPERCENTADDITIVE => "Physical Damage",
            UnitStat.UNITSTATABILITYPOWERPERCENTADDITIVE => "Special Damage",
            UnitStat.UNITSTATDODGENEGATEPERCENTADDITIVE => "Physical Accuracy",
            UnitStat.UNITSTATDEFLECTIONNEGATEPERCENTADDITIVE => "Special Accuracy",
            UnitStat.UNITSTATATTACKCRITICALNEGATEPERCENTADDITIVE => "Physical Critical Avoidance",
            UnitStat.UNITSTATABILITYCRITICALNEGATEPERCENTADDITIVE => "Special Critical Avoidance",
            UnitStat.UNITSTATDODGENEGATERATING => "Physical Accuracy",
            UnitStat.UNITSTATDEFLECTIONNEGATERATING => "Special Accuracy",
            UnitStat.UNITSTATATTACKCRITICALNEGATERATING => "Physical Critical Avoidance",
            UnitStat.UNITSTATABILITYCRITICALNEGATERATING => "Special Critical Avoidance",
            UnitStat.UNITSTATOFFENSE => "Offense",
            UnitStat.UNITSTATDEFENSE => "Defense",
            UnitStat.UNITSTATDEFENSEPENETRATION => "Defense Penetration",
            UnitStat.UNITSTATEVASIONRATING => "Evasion",
            UnitStat.UNITSTATCRITICALRATING => "Critical Chance",
            UnitStat.UNITSTATEVASIONNEGATERATING => "Accuracy",
            UnitStat.UNITSTATCRITICALNEGATERATING => "Critical Avoidance",
            UnitStat.UNITSTATOFFENSEPERCENTADDITIVE => "Offense",
            UnitStat.UNITSTATDEFENSEPERCENTADDITIVE => "Defense",
            UnitStat.UNITSTATDEFENSEPENETRATIONPERCENTADDITIVE => "Defense Penetration",
            UnitStat.UNITSTATEVASIONPERCENTADDITIVE => "Evasion",
            UnitStat.UNITSTATEVASIONNEGATEPERCENTADDITIVE => "Accuracy",
            UnitStat.UNITSTATCRITICALCHANCEPERCENTADDITIVE => "Critical Chance",
            UnitStat.UNITSTATCRITICALNEGATECHANCEPERCENTADDITIVE => "Critical Avoidance",
            UnitStat.UNITSTATMAXHEALTHPERCENTADDITIVE => "Health",
            UnitStat.UNITSTATMAXSHIELDPERCENTADDITIVE => "Protection",
            UnitStat.UNITSTATSPEEDPERCENTADDITIVE => "Speed",
            UnitStat.UNITSTATCOUNTERATTACKRATING => "Counter Attack",
            UnitStat.UNITSTATTAUNT => "UnitStat_Taunt",
            UnitStat.UNITSTATDEFENSEPENETRATIONTARGETPERCENTADDITIVE => "UnitStat_Defense_Penetration_Target_Percentage_Additive",
            UnitStat.UNITSTATMASTERY => "Mastery",
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
        yield return IsPercentage;
    }
}