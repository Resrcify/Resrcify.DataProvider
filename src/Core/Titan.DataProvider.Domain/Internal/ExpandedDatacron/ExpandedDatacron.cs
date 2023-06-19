using System.Collections.Generic;
using System.Runtime.InteropServices;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron.ValueObjects;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Titan.DataProvider.Domain.Shared;
using GameData = Titan.DataProvider.Domain.Internal.BaseData.BaseData;

namespace Titan.DataProvider.Domain.Internal.ExpandedDatacron;

public sealed class ExpandedDatacron
{
    public int SetId { get; private set; }
    public string SetName { get; private set; }
    public string Image { get; private set; }
    public int MaxTiers { get; private set; }
    public int ActivatedTiers { get; private set; }
    private readonly List<AbilityTier> _abilities = new();
    public IReadOnlyList<AbilityTier> Abilities => _abilities;
    private readonly List<StatTier> _stats = new();
    public IReadOnlyList<StatTier> Stats => _stats;

    private ExpandedDatacron(int setId, string setName, string iconKey, int maxTiers, int activatedTiers, List<AbilityTier> abilites, List<StatTier> stats)
    {
        SetId = setId;
        SetName = setName;
        Image = iconKey;
        MaxTiers = maxTiers;
        ActivatedTiers = activatedTiers;
        _abilities = abilites;
        _stats = stats;
    }

    public static Result<ExpandedDatacron> Create(int setId, string setName, string iconKey, int maxTiers, int activatedTiers, List<AbilityTier> abilites, List<StatTier> stats)
    {
        return new ExpandedDatacron(setId, setName, iconKey, maxTiers, activatedTiers, abilites, stats);
    }
    public static IEnumerable<ExpandedDatacron> Create(List<Datacron> playerDatacrons, GameData gameData)
    {
        foreach (var playerDatacron in playerDatacrons)
        {
            var setId = playerDatacron.SetId;
            var gameDataDatacron = gameData.Datacrons[setId];
            var setName = gameDataDatacron.NameKey;
            var maxTiers = gameDataDatacron.Tier.Count;
            var abilites = new List<AbilityTier>();
            var stats = new List<StatTier>();
            int tier = 1;
            foreach (var playerAffix in playerDatacron.Affix)
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
            var expandedDatacron = Create(setId, setName, gameDataDatacron.IconKey, maxTiers, activatedTiers, abilites, stats);
            yield return expandedDatacron.Value;
        }
    }
}