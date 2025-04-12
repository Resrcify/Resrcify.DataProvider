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
    public bool IsFocused { get; private set; }
    public string SetName { get; private set; }
    public string Image { get; private set; }
    public int MaxTiers { get; private set; }
    public int ActivatedTiers { get; private set; }
    private readonly List<AbilityTier> _abilities = [];
    public IReadOnlyList<AbilityTier> Abilities => _abilities;
    private readonly List<StatTier> _stats = [];
    public IReadOnlyList<StatTier> Stats => _stats;
    public int RerollCount { get; private set; }

    private ExpandedDatacron(
        string id,
        string templateId,
        int setId,
        bool isFocused,
        string setName,
        string iconKey,
        int maxTiers,
        int activatedTiers,
        List<AbilityTier> abilites,
        List<StatTier> stats,
        int rerollCount)
    {
        Id = id;
        TemplateId = templateId;
        SetId = setId;
        IsFocused = isFocused;
        SetName = setName;
        Image = iconKey;
        MaxTiers = maxTiers;
        ActivatedTiers = activatedTiers;
        _abilities = abilites;
        _stats = stats;
        RerollCount = rerollCount;
    }

    public static Result<ExpandedDatacron> Create(
        string id,
        string templateId,
        int setId,
        bool isFocused,
        string setName,
        string iconKey,
        int maxTiers,
        int activatedTiers,
        List<AbilityTier> abilites,
        List<StatTier> stats,
        int rerollCount)
        => new ExpandedDatacron(
            id,
            templateId,
            setId,
            isFocused,
            setName,
            iconKey,
            maxTiers,
            activatedTiers,
            abilites,
            stats,
            rerollCount);
    public static IEnumerable<ExpandedDatacron> Create(List<Datacron> playerDatacrons, GameData gameData)
    {
        foreach (var playerDatacron in playerDatacrons)
        {
            var id = playerDatacron.Id;
            if (id is null || playerDatacron.TemplateId is null)
                continue;

            var setId = playerDatacron.SetId;
            if (!gameData.Datacrons.TryGetValue(playerDatacron.TemplateId, out var gameDataDatacron))
                continue;
            var setName = gameDataDatacron.NameKey;
            var maxTiers = gameDataDatacron.Tier.Count;
            var abilites = new List<AbilityTier>();
            var stats = new List<StatTier>();
            int tier = 1;
            foreach (var playerAffix in playerDatacron.Affixs)
            {
                if (!string.IsNullOrEmpty(playerAffix.AbilityId) &&
                    !string.IsNullOrEmpty(playerAffix.TargetRule) &&
                    gameDataDatacron.Abilities.TryGetValue(playerAffix.AbilityId, out var gameDataAbility) &&
                    gameDataAbility.Targets.TryGetValue(playerAffix.TargetRule, out var gameDataAbilityTarget))
                {
                    var ability = AbilityTier.Create(tier, playerAffix, gameDataAbilityTarget);
                    if (ability.IsFailure)
                        continue;
                    abilites.Add(ability.Value);
                }
                else
                {
                    var stat = StatTier.Create(tier, playerAffix);
                    if (stat.IsFailure)
                        continue;
                    stats.Add(stat.Value);
                }
                tier++;
            }
            var activatedTiers = stats.Count + abilites.Count;
            var expandedDatacron = Create(
                id,
                playerDatacron.TemplateId,
                setId,
                gameDataDatacron.IsFocused,
                setName,
                gameDataDatacron.IconKey,
                maxTiers,
                activatedTiers,
                abilites,
                stats,
                playerDatacron.RerollCount);
            yield return expandedDatacron.Value;
        }
    }
}