using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Errors;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Resrcify.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;

public sealed class Stat : ValueObject
{
    public string Name { get; private set; }
    public UnitStat UnitStat { get; private set; }
    public double Base { get; private set; }
    public double Mod { get; private set; }
    public double Gear { get; private set; }
    public double Crew { get; private set; }
    public double Total { get; private set; }
    public bool IsPercentage { get; private set; }

    private Stat(string name, UnitStat unitStat, double baseValue, double gearValue, double modValue, double crewValue, bool isPercentage)
    {
        Name = name;
        UnitStat = unitStat;
        Base = baseValue;
        Gear = gearValue;
        Mod = modValue;
        Crew = crewValue;
        Total = baseValue + gearValue + modValue + crewValue;
        IsPercentage = isPercentage;
    }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
        yield return UnitStat;
        yield return Base;
        yield return Mod;
        yield return Total;
        yield return IsPercentage;

    }
    public static Result<Stat> Create(UnitStat unitStat, double baseValue, double gearValue, double modValue, double crewValue)
    {
        if (baseValue == 0 && gearValue == 0 && modValue == 0 && crewValue == 0)
            return Result.Failure<Stat>(DomainErrors.Stat.AllStatValuesZero);
        var baseStatValue = baseValue;
        var gearStatValue = gearValue;
        var modStatValue = modValue;
        var crewStatValue = crewValue;
        var isPercentage = EnumIsPercentage(unitStat);
        if (isPercentage)
        {
            baseStatValue *= 100;
            gearStatValue *= 100;
            modStatValue *= 100;
            crewStatValue *= 100;
        }
        return new Stat(GetInGameName(unitStat), unitStat, baseStatValue, gearStatValue, modStatValue, crewStatValue, isPercentage);
    }

    private static bool EnumIsPercentage(UnitStat enumValue)
         //As a rule of thumb:
         //Additive are percentages
         //Rating are flat values but normally calculated as percentages
         //Without additions are flat values
         => enumValue switch
         {
             UnitStat.Unitstatarmor or //Actually not percentage, this value is converted to percentage to mimic games presentation
             UnitStat.Unitstatsuppression or  //Actually not percentage, this value is converted to percentage to mimic games presentation
             UnitStat.Unitstatdodgerating or //Actually not percentage, this value is converted to percentage to mimic games presentation
             UnitStat.Unitstatdeflectionnegaterating or //Actually not percentage, this value is converted to percentage to mimic games presentation
             UnitStat.Unitstatattackcriticalrating or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
             UnitStat.Unitstatabilitycriticalrating or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
             UnitStat.Unitstatcriticaldamage or
             UnitStat.Unitstataccuracy or
             UnitStat.Unitstatresistance or
             UnitStat.Unitstatdodgepercentadditive or
             UnitStat.Unitstatdeflectionpercentadditive or
             UnitStat.Unitstatattackcriticalpercentadditive or
             UnitStat.Unitstatabilitycriticalpercentadditive or
             UnitStat.Unitstatarmorpercentadditive or
             UnitStat.Unitstatsuppressionpercentadditive or
             UnitStat.Unitstatarmorpenetrationpercentadditive or
             UnitStat.Unitstatsuppressionpenetrationpercentadditive or
             UnitStat.Unitstathealthsteal or
             UnitStat.Unitstatattackdamagepercentadditive or
             UnitStat.Unitstatabilitypowerpercentadditive or
             UnitStat.Unitstatdodgenegatepercentadditive or
             UnitStat.Unitstatdeflectionnegatepercentadditive or
             UnitStat.Unitstatattackcriticalnegatepercentadditive or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
             UnitStat.Unitstatabilitycriticalnegatepercentadditive or //Actually not percentage, however all moved to this value to handle the games using both flat and percentage types
             UnitStat.Unitstatdodgenegaterating or  //Actually not percentage, this value is converted to percentage to mimic games presentation
             UnitStat.Unitstatdeflectionnegaterating or  //Actually not percentage, this value is converted to percentage to mimic games presentation
             UnitStat.Unitstatattackcriticalnegaterating or
             UnitStat.Unitstatabilitycriticalnegaterating or
             UnitStat.Unitstatevasionrating or
             UnitStat.Unitstatcriticalrating or
             UnitStat.Unitstatevasionnegaterating or
             UnitStat.Unitstatcriticalnegaterating or
             UnitStat.Unitstatoffensepercentadditive or
             UnitStat.Unitstatdefensepercentadditive or
             UnitStat.Unitstatdefensepenetrationpercentadditive or
             UnitStat.Unitstatevasionpercentadditive or
             UnitStat.Unitstatevasionnegatepercentadditive or
             UnitStat.Unitstatcriticalchancepercentadditive or
             UnitStat.Unitstatcriticalnegatechancepercentadditive or
             UnitStat.Unitstatmaxhealthpercentadditive or
             UnitStat.Unitstatmaxshieldpercentadditive or
             UnitStat.Unitstatspeedpercentadditive or
             UnitStat.Unitstatcounterattackrating or
             UnitStat.Unitstatdefensepenetrationtargetpercentadditive
             => true,
             _ => false
         };


    private static string GetInGameName(UnitStat unitStat)
        => unitStat switch
        {
            UnitStat.Unitstatmaxhealth => "Health",
            UnitStat.Unitstatstrength => "Strength",
            UnitStat.Unitstatagility => "Agility",
            UnitStat.Unitstatintelligence => "Tactics",
            UnitStat.Unitstatspeed => "Speed",
            UnitStat.Unitstatattackdamage => "Physical Damage",
            UnitStat.Unitstatabilitypower => "Special Damage",
            UnitStat.Unitstatarmor => "Armor",
            UnitStat.Unitstatsuppression => "Resistance",
            UnitStat.Unitstatarmorpenetration => "Armor Penetration",
            UnitStat.Unitstatsuppressionpenetration => "Resistance Penetration",
            UnitStat.Unitstatdodgerating => "Dodge Chance",
            UnitStat.Unitstatdeflectionrating => "Deflection Chance",
            UnitStat.Unitstatattackcriticalrating => "Physical Critical Chance",
            UnitStat.Unitstatabilitycriticalrating => "Special Critical Chance",
            UnitStat.Unitstatcriticaldamage => "Critical Damage",
            UnitStat.Unitstataccuracy => "Potency",
            UnitStat.Unitstatresistance => "Tenacity",
            UnitStat.Unitstatdodgepercentadditive => "Dodge",
            UnitStat.Unitstatdeflectionpercentadditive => "Deflection",
            UnitStat.Unitstatattackcriticalpercentadditive => "Physical Critical Chance",
            UnitStat.Unitstatabilitycriticalpercentadditive => "Special Critical Chance",
            UnitStat.Unitstatarmorpercentadditive => "Armor",
            UnitStat.Unitstatsuppressionpercentadditive => "Resistance",
            UnitStat.Unitstatarmorpenetrationpercentadditive => "Armor Penetration",
            UnitStat.Unitstatsuppressionpenetrationpercentadditive => "Resistance Penetration",
            UnitStat.Unitstathealthsteal => "Health Steal",
            UnitStat.Unitstatmaxshield => "Protection",
            UnitStat.Unitstatshieldpenetration => "Protection Ignore",
            UnitStat.Unitstathealthregen => "Health Regeneration",
            UnitStat.Unitstatattackdamagepercentadditive => "Physical Damage",
            UnitStat.Unitstatabilitypowerpercentadditive => "Special Damage",
            UnitStat.Unitstatdodgenegatepercentadditive => "Physical Accuracy",
            UnitStat.Unitstatdeflectionnegatepercentadditive => "Special Accuracy",
            UnitStat.Unitstatattackcriticalnegatepercentadditive => "Physical Critical Avoidance",
            UnitStat.Unitstatabilitycriticalnegatepercentadditive => "Special Critical Avoidance",
            UnitStat.Unitstatdodgenegaterating => "Physical Accuracy",
            UnitStat.Unitstatdeflectionnegaterating => "Special Accuracy",
            UnitStat.Unitstatattackcriticalnegaterating => "Physical Critical Avoidance",
            UnitStat.Unitstatabilitycriticalnegaterating => "Special Critical Avoidance",
            UnitStat.Unitstatoffense => "Offense",
            UnitStat.Unitstatdefense => "Defense",
            UnitStat.Unitstatdefensepenetration => "Defense Penetration",
            UnitStat.Unitstatevasionrating => "Evasion",
            UnitStat.Unitstatcriticalrating => "Critical Chance",
            UnitStat.Unitstatevasionnegaterating => "Accuracy",
            UnitStat.Unitstatcriticalnegaterating => "Critical Avoidance",
            UnitStat.Unitstatoffensepercentadditive => "Offense",
            UnitStat.Unitstatdefensepercentadditive => "Defense",
            UnitStat.Unitstatdefensepenetrationpercentadditive => "Defense Penetration",
            UnitStat.Unitstatevasionpercentadditive => "Evasion",
            UnitStat.Unitstatevasionnegatepercentadditive => "Accuracy",
            UnitStat.Unitstatcriticalchancepercentadditive => "Critical Chance",
            UnitStat.Unitstatcriticalnegatechancepercentadditive => "Critical Avoidance",
            UnitStat.Unitstatmaxhealthpercentadditive => "Health",
            UnitStat.Unitstatmaxshieldpercentadditive => "Protection",
            UnitStat.Unitstatspeedpercentadditive => "Speed",
            UnitStat.Unitstatcounterattackrating => "Counter Attack",
            UnitStat.Unitstattaunt => "UnitStat_Taunt",
            UnitStat.Unitstatdefensepenetrationtargetpercentadditive => "UnitStat_Defense_Penetration_Target_Percentage_Additive",
            UnitStat.Unitstatmastery => "Mastery",
            _ => "None"
        };
}