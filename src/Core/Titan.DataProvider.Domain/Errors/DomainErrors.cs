using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Titan.DataProvider.Domain.Errors;

public static class DomainErrors
{

    public static class ExpandedUnit
    {
        public static readonly Error CombatTypeNotFound = new(
            "ExpandedUnit.CombatTypeNotFound",
            $"Undefined combat type. Unable to proceed.",
            ErrorType.Validation);
        public static readonly Error GameDataFileNotFound = new(
            "ExpandedUnit.GameDataFileNotFound",
            $"GameData file was not found. Please run the update endpoint, or wait for the periodic update.",
            ErrorType.NotFound);
    }
    public static class BaseData
    {
        public static readonly Error GameDataFileNotFound = new(
            "BaseData.GameDataFileNotFound",
            $"GameData file was not found. Please run the update endpoint, or wait for the periodic update.",
            ErrorType.NotFound);
    }
    public static class Stat
    {
        public static readonly Error AllStatValuesZero = new(
            "Stat.AllStatValuesZero",
            $"All stat values cannot be zero. Unable to proceed.",
            ErrorType.Validation);
    }
    public static class Skill
    {
        public static readonly Error UnableToFindSkillInGameData = new(
            "Skill.UnableToFindSkillInGameData",
            $"Specified skill is not found in the game data file.",
            ErrorType.NotFound);
    }
    public static class Mod
    {
        public static readonly Error PrimaryStatNotFound = new(
            "Mod.PrimaryStatNotFound",
            $"Specified primary stat was not found.",
            ErrorType.NotFound);
        public static readonly Error IdIsNull = new(
            "Mod.IdIsNull",
            $"A mods Id cannot be null.",
            ErrorType.Validation);
    }
    public static class ModStat
    {
        public static readonly Error UnableToCreate = new(
            "ModStat.UnableToCreate",
            $"Was not able to create ModStat.",
            ErrorType.Failure);
    }
}
