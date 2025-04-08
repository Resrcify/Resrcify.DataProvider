using System.Collections.Generic;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Resrcify.DataProvider.Domain.Internal.ExpandedDatacron.ValueObjects;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using GameData = Resrcify.DataProvider.Domain.Internal.BaseData.BaseData;

namespace Resrcify.DataProvider.Domain.Internal.ExpandedDatacron;

public sealed class ExpandedDatacron
{
    public string Id { get; private set; }
    public string TemplateId { get; private set; }
    public int SetId { get; private set; }
    public string SetName { get; private set; }
    public string Image { get; private set; }
    public int MaxTiers { get; private set; }
    public int ActivatedTiers { get; private set; }
    private readonly List<AbilityTier> _abilities = [];
    public IReadOnlyList<AbilityTier> Abilities => _abilities;
    private readonly List<StatTier> _stats = [];
    public IReadOnlyList<StatTier> Stats => _stats;
    public int RerollCount { get; private set; }

    private ExpandedDatacron(string id, string templateId, int setId, string setName, string iconKey, int maxTiers, int activatedTiers, List<AbilityTier> abilites, List<StatTier> stats, int rerollCount)
    {
        Id = id;
        TemplateId = templateId;
        SetId = setId;
        SetName = setName;
        Image = iconKey;
        MaxTiers = maxTiers;
        ActivatedTiers = activatedTiers;
        _abilities = abilites;
        _stats = stats;
        RerollCount = rerollCount;
    }

    public static Result<ExpandedDatacron> Create(string id, string templateId, int setId, string setName, string iconKey, int maxTiers, int activatedTiers, List<AbilityTier> abilites, List<StatTier> stats, int rerollCount)
    {
        return new ExpandedDatacron(id, templateId, setId, setName, iconKey, maxTiers, activatedTiers, abilites, stats, rerollCount);
    }
    public static IEnumerable<ExpandedDatacron> Create(List<Datacron> playerDatacrons, GameData gameData)
    {
        foreach (var playerDatacron in playerDatacrons)
        {
            var id = playerDatacron.Id;
            if (id is null || playerDatacron.TemplateId is null)
                continue;

            var setId = playerDatacron.SetId;
            var gameDataDatacron = gameData.Datacrons[playerDatacron.TemplateId];
            var setName = gameDataDatacron.NameKey;
            var maxTiers = gameDataDatacron.Tier.Count;
            var abilites = new List<AbilityTier>();
            var stats = new List<StatTier>();
            int tier = 1;
            foreach (var playerAffix in playerDatacron.Affixs)
            {
                if (tier is 1 or 2 or 4 or 5 or 7 or 8)
                {
                    var stat = StatTier.Create(tier, playerAffix);
                    stats.Add(stat.Value);
                }

                if (tier is 3 or 6 or 9)
                {
                    var gameDataAbility = gameDataDatacron.Abilities[playerAffix.AbilityId!].Targets[playerAffix.TargetRule!];
                    var ability = AbilityTier.Create(tier, playerAffix, gameDataAbility);
                    abilites.Add(ability.Value);
                }
                tier++;
            }
            var activatedTiers = stats.Count + abilites.Count;
            var expandedDatacron = Create(id, playerDatacron.TemplateId, setId, setName, gameDataDatacron.IconKey, maxTiers, activatedTiers, abilites, stats, playerDatacron.RerollCount);
            yield return expandedDatacron.Value;
        }
    }
}