using System.Linq;
using System.Collections.Generic;
using PlayerSkill = Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile.Skill;
using Unit = Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile.Unit;
using Resrcify.DataProvider.Domain.Errors;
using UnitData = Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData.UnitData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Resrcify.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;

public sealed class Skill : ValueObject
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string NameKey { get; private set; }
    public string Image { get; private set; }
    public int Tier { get; private set; }
    public int MaxTier { get; private set; }
    public bool HasActivatedZeta { get; private set; }
    public int ZetaTier { get; private set; }
    public bool HasActivatedOmicron { get; private set; }
    public int OmicronTier { get; private set; }
    public OmicronMode OmicronRestriction { get; private set; }
    public string OmicronRestrictionName { get; private set; }

    private Skill(
        string id,
        string name,
        string nameKey,
        string image,
        int tier,
        int maxTier,
        bool hasActivatedZeta,
        int zetaTier,
        bool hasActivatedOmicron,
        int omicronTier,
        OmicronMode omicronMode,
        string omicronModeName)
    {
        Id = id;
        Name = name;
        NameKey = nameKey;
        Image = image;
        Tier = tier;
        MaxTier = maxTier;
        HasActivatedOmicron = hasActivatedOmicron;
        OmicronTier = omicronTier;
        HasActivatedZeta = hasActivatedZeta;
        ZetaTier = zetaTier;
        OmicronRestriction = omicronMode;
        OmicronRestrictionName = omicronModeName;
    }

    public static Result<Skill> Create(PlayerSkill skill, UnitData data)
    {
        var skillData = data.Skills.FirstOrDefault(x => x.Id == skill.Id);
        if (skillData is null)
            return Result.Failure<Skill>(DomainErrors.Skill.UnableToFindSkillInGameData);
        bool hasActivatedZeta = false;
        bool hasActivatedOmicron = false;
        var skillTier = skill.Tier + 2;

        if (skillData.IsZeta && skillTier >= skillData.ZetaTier)
            hasActivatedZeta = true;
        if (skillData.IsOmicron && skillTier >= skillData.OmicronTier)
            hasActivatedOmicron = true;

        return new Skill(skillData.Id, skillData.Name, skillData.NameKey, skillData.Image, skillTier, (int)skillData.MaxTier, hasActivatedZeta, skillData.ZetaTier, hasActivatedOmicron, skillData.OmicronTier, skillData.OmicronMode, skillData.OmicronModeName);
    }
    public static Result<List<Skill>> Create(Unit unit, UnitData data)
    {
        var skillDict = new List<Skill>();
        foreach (var skill in unit.Skills)
        {
            var newSkill = Create(skill, data);
            if (newSkill.IsSuccess)
                skillDict.Add(newSkill.Value);
        }
        return skillDict;
    }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return NameKey;
        yield return Image;
        yield return Tier;
        yield return MaxTier;
        yield return HasActivatedZeta;
        yield return ZetaTier;
        yield return HasActivatedOmicron;
        yield return OmicronTier;
        yield return OmicronRestriction;
        yield return OmicronRestrictionName;
    }
}