using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Shouldly;
using Xunit;
using DatacronDataValue = Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData.DatacronData;

namespace Resrcify.DataProvider.Domain.UnitTests.Internal.BaseData.ValueObjects.DatacronData;

/// <summary>
/// A datacron template's stats come from its stat affixes only. The game data is set 36's (The Legend Codex) in the
/// shape SwgohApi answers it: an ability affix carries an ability id and, since the proto2 contract
/// (Resrcify.SwgohApi 3.12.0), the unset <c>stat_type</c>'s declared default, <c>UNITSTATMAXHEALTH</c> (1).
/// </summary>
public sealed class DatacronDataTests
{
    private const string TemplateId = "datacron_set_36_base";

    [Fact]
    public void Create_Should_TakeStatsFromStatAffixesOnly_When_AnAbilityAffixReadsAsHealth()
    {
        var gameData = ReadGameData();

        var datacrons = DatacronDataValue.Create(gameData, Localization());

        datacrons.IsSuccess.ShouldBeTrue();
        var datacron = datacrons.Value[TemplateId];
        datacron.Stats.Keys.ShouldBe(["49", "55", "56"], ignoreOrder: true);
        datacron.Stats.ShouldNotContainKey("1");
    }

    [Fact]
    public void Create_Should_KeepTheAbilityAffix_When_ItReadsAsHealth()
    {
        var gameData = ReadGameData();

        var datacrons = DatacronDataValue.Create(gameData, Localization());

        var datacron = datacrons.Value[TemplateId];
        datacron.Abilities.Keys.ShouldBe(["datacron_alignment_generic_036"]);
        datacron.Abilities["datacron_alignment_generic_036"].Targets.Keys.ShouldBe(["target_datacron_darkside"]);
    }

    private static GameDataResponse ReadGameData()
    {
        var json = File.ReadAllText(Path.Combine("Fixtures", "datacron-set-36-gamedata.json"));
        return JsonSerializer.Deserialize<GameDataResponse>(json)!;
    }

    /// <summary>
    /// The localization the datacron's names come from. It names Health too (<c>UnitStat_MaxHealth</c>), so a Health
    /// stat would be kept if an ability affix were taken for one.
    /// </summary>
    private static Dictionary<string, string> Localization()
        => new()
        {
            ["DATACRON_SET_36_NAME"] = "The Legend Codex",
            ["DATACRON_ALIGNMENT_GENERIC_036_NAME"] = "Bonus mechanics for {0} characters",
            ["DATACRON_ALIGNMENT_GENERIC_036_DESC"] = "Whenever {0} allies resist a debuff, they recover 10% Health and Protection.",
            ["CATEGORY_ALIGNMENT_DARK_NAME"] = "Dark Side",
            ["UnitStat_MaxHealth"] = "Health",
            ["UnitStat_DefensePercentAdditive"] = "Defense",
            ["UnitStat_MaxHealthPercentAdditive"] = "Health",
            ["UnitStat_MaxShieldPercentAdditive"] = "Protection",
        };
}
