using System.IO;
using System.Linq;
using System.Text.Json;
using Resrcify.DataProvider.Application.Extensions;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;
using Shouldly;
using Xunit;
using BaseDataAggregate = Resrcify.DataProvider.Domain.Internal.BaseData.BaseData;
using ExpandedDatacronValue = Resrcify.DataProvider.Domain.Internal.ExpandedDatacron.ExpandedDatacron;

namespace Resrcify.DataProvider.Domain.UnitTests.Internal.ExpandedDatacron;

/// <summary>
/// A player's datacron expanded against the cached game data of set 36 (production's cached BaseData, trimmed to
/// two templates). The player's affixes are in the shape SwgohApi answers them: an ability affix carries an ability
/// id, no stat value and, since the proto2 contract (Resrcify.SwgohApi 3.12.0), the unset <c>stat_type</c>'s
/// declared default, <c>UNITSTATMAXHEALTH</c> (1).
/// </summary>
public sealed class ExpandedDatacronTests
{
    private const string TemplateId = "datacron_set_36_base";
    private const string DarkSideRule = "target_datacron_darkside";

    [Fact]
    public void Create_Should_ExpandAbilityAndStatAffixes_When_TheAbilityAffixReadsAsHealth()
    {
        var datacron = PlayerDatacron(
            StatAffix(UnitStat.Unitstatdefensepercentadditive, 40000000),
            StatAffix(UnitStat.Unitstatmaxshieldpercentadditive, 7500000),
            AbilityAffix("datacron_alignment_generic_036"));

        var expanded = ExpandedDatacronValue.Create([datacron], ReadBaseData()).Single();

        expanded.Stats.Select(stat => stat.UnitStat).ShouldBe(
            [UnitStat.Unitstatdefensepercentadditive, UnitStat.Unitstatmaxshieldpercentadditive]);
        expanded.Abilities.Select(ability => (ability.AbilityId, ability.Tier)).ShouldBe(
            [("datacron_alignment_generic_036", 3)]);
        expanded.ActivatedTiers.ShouldBe(3);
    }

    [Fact]
    public void Create_Should_NotTakeAnAbilityAffixForAHealthStat_When_TheGameDataHasNoSuchAbility()
    {
        var datacron = PlayerDatacron(
            StatAffix(UnitStat.Unitstatdefensepercentadditive, 40000000),
            StatAffix(UnitStat.Unitstatmaxshieldpercentadditive, 7500000),
            AbilityAffix("datacron_alignment_generic_999"));

        var expanded = ExpandedDatacronValue.Create([datacron], ReadBaseData()).Single();

        expanded.Stats.ShouldNotContain(stat => stat.UnitStat == UnitStat.Unitstatmaxhealth);
        expanded.Stats.Count.ShouldBe(2);
        expanded.Abilities.ShouldBeEmpty();
        expanded.ActivatedTiers.ShouldBe(3);
    }

    private static BaseDataAggregate ReadBaseData()
    {
        var json = File.ReadAllText(Path.Combine("Fixtures", "datacron-set-36-basedata.json"));
        return JsonSerializer.Deserialize<BaseDataAggregate>(json, JsonSerializerExtensions.GetDomainSerializerOptions())!;
    }

    private static Datacron PlayerDatacron(params DatacronAffix[] affixes)
        => new()
        {
            Id = "datacron-36-player-1",
            SetId = 36,
            TemplateId = TemplateId,
            Affixs = [.. affixes],
        };

    private static DatacronAffix StatAffix(UnitStat statType, long statValue)
        => new()
        {
            AbilityId = string.Empty,
            TargetRule = string.Empty,
            StatType = statType,
            StatValue = statValue,
            RequiredUnitTier = UnitTier.Tier02,
            RequiredRelicTier = RelicTier.Reliclocked,
        };

    private static DatacronAffix AbilityAffix(string abilityId)
        => new()
        {
            AbilityId = abilityId,
            TargetRule = DarkSideRule,
            StatType = UnitStat.Unitstatmaxhealth,
            StatValue = 0,
            RequiredUnitTier = UnitTier.Tier02,
            RequiredRelicTier = RelicTier.Reliclocked,
            ScopeIcon = "tex.datacronui_affix_darkside",
        };
}
