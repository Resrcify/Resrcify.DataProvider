using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Errors;

public static class DomainErrors
{

    public static class ExpandedUnit
    {
        public static readonly Error CombatTypeNotFound = new(
            "ExpandedUnit.CombatTypeNotFound",
            $"Undefined combat type. Unable to proceed.");
        public static readonly Error GameDataFileNotFound = new(
            "ExpandedUnit.GameDataFileNotFound",
            $"GameData file was not found. Please run the update endpoint, or wait for the periodic update.");
    }
    public static class BaseData
    {
        public static readonly Error GameDataFileNotFound = new(
            "BaseData.GameDataFileNotFound",
            $"GameData file was not found. Please run the update endpoint, or wait for the periodic update.");
    }
    public static class Stat
    {
        public static readonly Error AllStatValuesZero = new(
            "Stat.AllStatValuesZero",
            $"All stat values cannot be zero. Unable to proceed.");
    }
    public static class Skill
    {
        public static readonly Error UnableToFindSkillInGameData = new(
            "Skill.UnableToFindSkillInGameData",
            $"Specified skill is not found in the game data file.");
    }
    public static class Mod
    {
        public static readonly Error PrimaryStatNotFound = new(
            "Mod.PrimaryStatNotFound",
            $"Specified primary stat was not found.");
    }
    public static class ModStat
    {
        public static readonly Error UnableToCreate = new(
            "ModStat.UnableToCreate",
            $"Was not able to create ModStat.");
    }
}
