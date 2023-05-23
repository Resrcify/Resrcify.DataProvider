using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects
{
    public sealed class Stat : ValueObject
    {
        public string Name { get; private set; }
        public UnitStat UnitStat { get; private set; }
        public double BaseValue { get; private set; }
        public double ModValue { get; private set; }
        public double TotalValue { get; private set; }
        public bool IsPercentage { get; private set; }

        public Stat(string name, UnitStat unitStat, double baseValue, double modValue, bool isPercentage)
        {
            Name = name;
            UnitStat = unitStat;
            BaseValue = baseValue;
            ModValue = modValue;
            TotalValue = BaseValue + ModValue;
            IsPercentage = isPercentage;
        }
        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return UnitStat;
            yield return BaseValue;
            yield return ModValue;
            yield return TotalValue;
            yield return IsPercentage;

        }
        public static Result<Stat> Create(int enumValue, double baseValue, double modValue)
        {

            return new Stat(GetInGameName(enumValue), (UnitStat)enumValue, baseValue, modValue, CheckIfPercentage(enumValue));
        }

        private static bool CheckIfPercentage(int enumValue)
            => enumValue switch
            {
                8 or 9 or 14 or 15 or 17 or 18 or 27 or 12 or 13 or 16 or 35 or 36 or 39 or 40 => true,
                _ => false
            };

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



    }
}