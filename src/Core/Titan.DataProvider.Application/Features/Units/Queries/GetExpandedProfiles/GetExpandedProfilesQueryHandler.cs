using System.Threading;
using System.Threading.Tasks;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Domain.Internal.BaseData;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron;
using System.Linq;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Enums;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.Common;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.SharedKernel.ResultFramework.Primitives;

namespace Titan.DataProvider.Application.Features.Units.Queries.GetExpandedProfiles;

public sealed class GetExpandedProfilesQueryHandler : IQueryHandler<GetExpandedProfilesQuery, IEnumerable<GetExpandedProfilesQueryResponse>>
{
    private readonly ICachingService _caching;
    public GetExpandedProfilesQueryHandler(ICachingService caching)
        => _caching = caching;

    public async Task<Result<IEnumerable<GetExpandedProfilesQueryResponse>>> Handle(GetExpandedProfilesQuery request, CancellationToken cancellationToken)
    {
        var baseData = await _caching.GetAsync<BaseData>($"BaseData-{request.Language}", cancellationToken);
        if (baseData is null)
            return Result.Success(Enumerable.Empty<GetExpandedProfilesQueryResponse>());

        var expandedProfiles = GetExpandedPlayerProfiles(request, baseData);
        return Result.Success(expandedProfiles);
    }

    private static IEnumerable<GetExpandedProfilesQueryResponse> GetExpandedPlayerProfiles(GetExpandedProfilesQuery request, BaseData baseData)
    {
        foreach (var profile in request.PlayerProfiles)
        {
            var units = ExpandedUnit.Create(profile, request.WithStats, request.WithoutGp, request.WithoutModStats, request.WithoutMods, request.WithoutSkills, baseData);
            var datacrons = Enumerable.Empty<ExpandedDatacron>();
            if (!request.WithoutDatacrons)
                datacrons = ExpandedDatacron.Create(profile.Datacrons, baseData);
            var datacronSummary = ParseDatacronSummary(datacrons);
            var summary = ParseSummaryData(units, datacronSummary);
            yield return new GetExpandedProfilesQueryResponse(profile.PlayerId ?? "Unknown", profile.AllyCode, profile.Name ?? "Unknown", summary, units.ToDictionary(x => x.Key, x => x.Value), datacrons);
        }
    }
    private static DatacronSummary ParseDatacronSummary(IEnumerable<ExpandedDatacron> expandedDatacrons)
    {
        var level0 = 0;
        var level1 = 0;
        var level2 = 0;
        var level3 = 0;
        var level4 = 0;
        var level5 = 0;
        var level6 = 0;
        var level7 = 0;
        var level8 = 0;
        var level9 = 0;
        var rerolls = 0;
        foreach (var datacron in expandedDatacrons)
        {
            if (datacron.ActivatedTiers == 0)
                level0++;
            if (datacron.ActivatedTiers == 1)
                level1++;
            if (datacron.ActivatedTiers == 2)
                level2++;
            if (datacron.ActivatedTiers == 3)
                level3++;
            if (datacron.ActivatedTiers == 4)
                level4++;
            if (datacron.ActivatedTiers == 5)
                level5++;
            if (datacron.ActivatedTiers == 6)
                level6++;
            if (datacron.ActivatedTiers == 7)
                level7++;
            if (datacron.ActivatedTiers == 8)
                level8++;
            if (datacron.ActivatedTiers == 9)
                level9++;
            rerolls += datacron.RerollCount;
        }
        return new DatacronSummary(level0, level1, level2, level3, level4, level5, level6, level7, level8, level9, rerolls);
    }
    private static ProfileSummary ParseSummaryData(IEnumerable<KeyValuePair<string, ExpandedUnit>> expandedUnits, DatacronSummary datacronSummary)
    {
        var glCount = 0;
        var characters = 0;
        var characterGp = 0.0;
        var ships = 0;
        var shipGp = 0.0;

        var sixDotModsCount = 0;
        var speedUnder10 = 0;
        var speedBetween10And14 = 0;
        var speedBetween15And19 = 0;
        var speedBetween20And24 = 0;
        var speed25Plus = 0;
        var offenceBetween4And6Percent = 0;
        var offenceOver6Percent = 0;

        var zetaCount = 0;
        var omicronCount = 0;
        var tbOmicronCount = 0;
        var twOmicronCount = 0;
        var gaOmicronCount = 0;
        var cqOmicronCount = 0;
        var raidOmicronCount = 0;

        var relic0 = 0;
        var relic1 = 0;
        var relic2 = 0;
        var relic3 = 0;
        var relic4 = 0;
        var relic5 = 0;
        var relic6 = 0;
        var relic7 = 0;
        var relic8 = 0;
        var relic9 = 0;
        var relicCount = 0;

        var gear1 = 0;
        var gear2 = 0;
        var gear3 = 0;
        var gear4 = 0;
        var gear5 = 0;
        var gear6 = 0;
        var gear7 = 0;
        var gear8 = 0;
        var gear9 = 0;
        var gear10 = 0;
        var gear11 = 0;
        var gear12 = 0;
        var gear13 = 0;

        foreach (var unit in expandedUnits)
        {
            if (unit.Value.CombatType == CombatType.Character)
            {
                characters++;
                characterGp += unit.Value.Gp;
            }
            if (unit.Value.CombatType == CombatType.Ship)
            {
                ships++;
                shipGp += unit.Value.Gp;
            }
            if (unit.Value.IsGalacticLegend)
                glCount++;

            ExtractModData(ref sixDotModsCount, ref speedUnder10, ref speedBetween10And14, ref speedBetween15And19, ref speedBetween20And24, ref speed25Plus, ref offenceBetween4And6Percent, ref offenceOver6Percent, unit);
            ExtractSkillData(ref zetaCount, ref omicronCount, ref tbOmicronCount, ref twOmicronCount, ref gaOmicronCount, ref cqOmicronCount, ref raidOmicronCount, unit);

            if (unit.Value.GearTier == UnitTier.Tier01)
                gear1++;
            if (unit.Value.GearTier == UnitTier.Tier02)
                gear2++;
            if (unit.Value.GearTier == UnitTier.Tier03)
                gear3++;
            if (unit.Value.GearTier == UnitTier.Tier04)
                gear4++;
            if (unit.Value.GearTier == UnitTier.Tier05)
                gear5++;
            if (unit.Value.GearTier == UnitTier.Tier06)
                gear6++;
            if (unit.Value.GearTier == UnitTier.Tier07)
                gear7++;
            if (unit.Value.GearTier == UnitTier.Tier08)
                gear8++;
            if (unit.Value.GearTier == UnitTier.Tier09)
                gear9++;
            if (unit.Value.GearTier == UnitTier.Tier10)
                gear10++;
            if (unit.Value.GearTier == UnitTier.Tier11)
                gear11++;
            if (unit.Value.GearTier == UnitTier.Tier12)
                gear12++;
            if (unit.Value.GearTier == UnitTier.Tier13)
                gear13++;

            if (unit.Value.RelicTier < RelicTier.Reliclocked)
                continue;
            relicCount++;
            if (unit.Value.RelicTier == RelicTier.Relicunlocked)
                relic0++;
            if (unit.Value.RelicTier == RelicTier.Relictier01)
                relic1++;
            if (unit.Value.RelicTier == RelicTier.Relictier02)
                relic2++;
            if (unit.Value.RelicTier == RelicTier.Relictier03)
                relic3++;
            if (unit.Value.RelicTier == RelicTier.Relictier04)
                relic4++;
            if (unit.Value.RelicTier == RelicTier.Relictier05)
                relic5++;
            if (unit.Value.RelicTier == RelicTier.Relictier06)
                relic6++;
            if (unit.Value.RelicTier == RelicTier.Relictier07)
                relic7++;
            if (unit.Value.RelicTier == RelicTier.Relictier08)
                relic8++;
            if (unit.Value.RelicTier == RelicTier.Relictier09)
                relic9++;

        }

        var gear = new GearSummary(gear1 - ships, gear2, gear3, gear4, gear5, gear6, gear7, gear8, gear9, gear10, gear11, gear12, gear13); // Ships are all GearTier1
        var relic = new RelicSummary(relic0, relic1, relic2, relic3, relic4, relic5, relic6, relic7, relic8, relic9, relicCount);

        var mods = new ModSummary(
            sixDotModsCount,
            speedUnder10,
            speedBetween10And14,
            speedBetween15And19,
            speedBetween20And24,
            speed25Plus,
            offenceBetween4And6Percent,
            offenceOver6Percent);

        var omicrons = new OmicronSummary(
            tbOmicronCount,
            twOmicronCount,
            gaOmicronCount,
            cqOmicronCount,
            raidOmicronCount,
            omicronCount);

        return new ProfileSummary(glCount, characters, ships, characterGp, shipGp, characterGp + shipGp, gear, relic, zetaCount, omicrons, mods, datacronSummary
        );
    }

    private static void ExtractModData(ref int sixDotModsCount, ref int speedUnder10, ref int speedBetween10And14, ref int speedBetween15And19, ref int speedBetween20And24, ref int speed25Plus, ref int offenceBetween4And6Percent, ref int offenceOver6Percent, KeyValuePair<string, ExpandedUnit> unit)
    {
        foreach (var mod in unit.Value.Mods)
        {
            if (mod.Rarity == ModRarity.MODRARITY6)
                sixDotModsCount++;
            foreach (var secondaryStat in mod.SecondaryStats)
            {
                if (secondaryStat.UnitStat == UnitStat.Unitstatspeed && secondaryStat.Value < 10)
                    speedUnder10++;
                if (secondaryStat.UnitStat == UnitStat.Unitstatspeed && secondaryStat.Value >= 10 && secondaryStat.Value <= 14)
                    speedBetween10And14++;
                if (secondaryStat.UnitStat == UnitStat.Unitstatspeed && secondaryStat.Value >= 15 && secondaryStat.Value <= 19)
                    speedBetween15And19++;
                if (secondaryStat.UnitStat == UnitStat.Unitstatspeed && secondaryStat.Value >= 20 && secondaryStat.Value <= 24)
                    speedBetween20And24++;
                if (secondaryStat.UnitStat == UnitStat.Unitstatspeed && secondaryStat.Value >= 25)
                    speed25Plus++;
                if (secondaryStat.UnitStat == UnitStat.Unitstatoffensepercentadditive && secondaryStat.Value >= 6)
                    offenceOver6Percent++;
                if (secondaryStat.UnitStat == UnitStat.Unitstatoffensepercentadditive && secondaryStat.Value >= 4 && secondaryStat.Value < 6)
                    offenceBetween4And6Percent++;
            }
        }
    }

    private static void ExtractSkillData(ref int zetaCount, ref int omicronCount, ref int tbOmicronCount, ref int twOmicronCount, ref int gaOmicronCount, ref int cqOmicronCount, ref int raidOmicronCount, KeyValuePair<string, ExpandedUnit> unit)
    {
        foreach (var skill in unit.Value.Skills)
        {
            if (skill.HasActivatedZeta)
                zetaCount++;
            if (!skill.HasActivatedOmicron)
                continue;

            omicronCount++;

            if (skill.OmicronRestriction == OmicronMode.Territorybattlebothomicron ||
                skill.OmicronRestriction == OmicronMode.Territorycovertomicron ||
                skill.OmicronRestriction == OmicronMode.Territorystrikeomicron)
                tbOmicronCount++;
            if (skill.OmicronRestriction == OmicronMode.Territorytournamentomicron ||
                skill.OmicronRestriction == OmicronMode.Territorytournament3omicron ||
                skill.OmicronRestriction == OmicronMode.Territorytournament5omicron)
                gaOmicronCount++;
            if (skill.OmicronRestriction == OmicronMode.Territorywaromicron)
                twOmicronCount++;
            if (skill.OmicronRestriction == OmicronMode.Conquestomicron)
                cqOmicronCount++;
            if (skill.OmicronRestriction == OmicronMode.Guildraidomicron)
                raidOmicronCount++;
        }
    }
}
