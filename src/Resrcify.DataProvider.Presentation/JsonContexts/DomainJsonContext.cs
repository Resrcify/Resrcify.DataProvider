using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.ExpandedDatacron;
using Resrcify.DataProvider.Domain.Internal.ExpandedDatacron.ValueObjects;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit;
using Skill = Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData.Skill;
using Stat = Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData.Stat;
using PStat = Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.Common.Stat;
using PSkill = Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile.Skill;
using System.Collections.Generic;
using Resrcify.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;
using Resrcify.DataProvider.Application.Features.Data.Queries.GetCachedLocalizationData;
using Resrcify.DataProvider.Application.Features.Data.Queries.GetCachedBaseData;
using Resrcify.DataProvider.Application.Features.Units.Queries.GetExpandedProfile;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.PlayerProfile;

namespace Resrcify.DataProvider.Presentation.JsonContexts;
[JsonSerializable(typeof(GetCachedLocalizationDataQueryRequest))]
[JsonSerializable(typeof(GetCachedBaseDataQueryRequest))]
[JsonSerializable(typeof(GetExpandedProfileQueryRequest))]
[JsonSerializable(typeof(PlayerProfileResponse))]
[JsonSerializable(typeof(ExpandedDatacron))]
[JsonSerializable(typeof(AbilityTier))]
[JsonSerializable(typeof(ExpandedUnit))]
[JsonSerializable(typeof(Skill), TypeInfoPropertyName = "InternalSkill")]
[JsonSerializable(typeof(List<Skill>), TypeInfoPropertyName = "InternalListSkill")]
[JsonSerializable(typeof(Stat), TypeInfoPropertyName = "InternalStat")]
[JsonSerializable(typeof(List<Stat>), TypeInfoPropertyName = "InternalListStat")]
[JsonSerializable(typeof(PSkill), TypeInfoPropertyName = "PlayerSkill")]
[JsonSerializable(typeof(List<PSkill>), TypeInfoPropertyName = "PlayerListSkill")]
[JsonSerializable(typeof(PStat), TypeInfoPropertyName = "PlayerStat")]
[JsonSerializable(typeof(List<PStat>), TypeInfoPropertyName = "PlayerListStat")]
[JsonSerializable(typeof(StatTier))]
[JsonSerializable(typeof(Mod))]
[JsonSerializable(typeof(ModStat))]
public partial class DomainJsonContext : JsonSerializerContext
{
}