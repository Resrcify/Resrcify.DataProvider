using System.Collections.Generic;
using System.Linq;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

public sealed class Skill : ValueObject
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string NameKey { get; private set; }
    public long MaxTier { get; private set; }
    public long Type { get; private set; }
    public string Image { get; private set; }
    public Dictionary<string, string> PowerOverrideTags { get; private set; }
    public bool IsZeta { get; private set; }
    public int ZetaTier { get; private set; }
    public bool IsOmicron { get; private set; }
    public int OmicronTier { get; private set; }
    public OmicronMode OmicronMode { get; private set; }
    public string OmicronModeName { get; private set; }
    private Skill(string id, string name, string nameKey, int maxTier, long type, string image, Dictionary<string, string> powerOverrideTags, bool isZeta, int zetaTier, bool isOmicron, int omicronTier, OmicronMode omicronMode, string omicronModeName)
    {
        Id = id;
        Name = name;
        NameKey = nameKey;
        MaxTier = maxTier;
        Type = type;
        Image = image;
        PowerOverrideTags = powerOverrideTags;
        IsZeta = isZeta;
        ZetaTier = zetaTier;
        IsOmicron = isOmicron;
        OmicronTier = omicronTier;
        OmicronMode = omicronMode;
        OmicronModeName = omicronModeName;
    }
    public static Result<Skill> Create(string id, string name, string nameKey, int maxTier, long type, string image, Dictionary<string, string> powerOverrideTags, bool isZeta, int zetaTier, bool isOmicron, int omicronTier, OmicronMode omicronMode, string omicronModeName)
    {
        return new Skill(id, name, nameKey, maxTier, type, image, powerOverrideTags, isZeta, zetaTier, isOmicron, omicronTier, omicronMode, omicronModeName);
    }

    public static Result<Dictionary<string, Skill>> Create(GameDataResponse data, Dictionary<string, string> local)
    {
        var skills = new Dictionary<string, Skill>();
        foreach (var skill in data.Skills)
        {
            var ability = data.Abilities.Find(a => a.Id == skill.AbilityReference);
            var powerOverrideTags = new Dictionary<string, string>();
            foreach (var tier in skill.Tiers.Select((Value, i) => new { i, Value }))
            {
                if (!string.IsNullOrEmpty(tier.Value.PowerOverrideTag))
                    powerOverrideTags[(tier.i + 2).ToString()] = tier.Value.PowerOverrideTag;
            }

            int omicronTier = 1;
            int zetaTier = 1;
            bool isZeta = false;
            bool isOmicron = false;

            foreach (var tier in skill.Tiers)
            {
                omicronTier++;
                if (tier.IsOmicronTier)
                {
                    isOmicron = true;
                    break;
                }
            }
            foreach (var tier in skill.Tiers)
            {
                zetaTier++;
                if (tier.IsZetaTier)
                {
                    isZeta = true;
                    break;
                }
            }

            var omicronMode = skill.OmicronMode;
            var omicronModeName = GetInGameName(omicronMode);
            skills[skill.Id!] = Create(
                skill.Id!,
                local[ability!.NameKey!],
                ability.NameKey!,
                skill.Tiers.Count + 1,
                (long)skill.SkillType,
                ability.Icon!,
                powerOverrideTags,
                isZeta,
                isZeta ? zetaTier : 0,
                isOmicron,
                isOmicron ? omicronTier : 0,
                omicronMode,
                omicronModeName
            ).Value;


        }
        return skills;
    }

    private static string GetInGameName(OmicronMode mode)
      => mode switch
      {
          //OmicronMode.ALLOMICRON => "Everywhere",
          OmicronMode.Pveomicron => "PvE Content",
          OmicronMode.Pvpomicron => "PvP Content",
          OmicronMode.Guildraidomicron => "Raids",
          OmicronMode.Territorystrikeomicron => "Territory Battles (Combat Missions)",
          OmicronMode.Territorycovertomicron => "Territory Battles (Special Missions)",
          OmicronMode.Territorybattlebothomicron => "Territory Battles",
          OmicronMode.Territorywaromicron => "Territory Wars",
          OmicronMode.Territorytournamentomicron => "Grand Arena",
          OmicronMode.Waromicron => "Galactic War",
          OmicronMode.Conquestomicron => "Conquest",
          OmicronMode.Galacticchallengeomicron => "Galactic Challenges",
          OmicronMode.Pveeventomicron => "Events",
          OmicronMode.Territorytournament3omicron => "Grand Arena (3v3)",
          OmicronMode.Territorytournament5omicron => "Grand Arena (5v5)",
          _ => "None"
      };

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return NameKey;
        yield return MaxTier;
        yield return Type;
        yield return Image;
        yield return PowerOverrideTags;
        yield return IsZeta;
        yield return ZetaTier;
        yield return IsOmicron;
        yield return OmicronTier;
        yield return OmicronMode;
    }
}