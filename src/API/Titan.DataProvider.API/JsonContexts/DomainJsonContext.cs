using System.Text.Json.Serialization;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron;
using Titan.DataProvider.Domain.Internal.ExpandedDatacron.ValueObjects;
using Titan.DataProvider.Domain.Internal.ExpandedUnit;
using Skill = Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData.Skill;
using Stat = Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData.Stat;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.ValueObjects;

namespace Titan.DataProvider.Application.JsonContexts;
[JsonSerializable(typeof(ExpandedDatacron))]
[JsonSerializable(typeof(AbilityTier))]
[JsonSerializable(typeof(ExpandedUnit))]
[JsonSerializable(typeof(Skill), TypeInfoPropertyName = "InternalSkill")]
[JsonSerializable(typeof(List<Skill>), TypeInfoPropertyName = "InternalListSkill")]
[JsonSerializable(typeof(Stat), TypeInfoPropertyName = "InternalStat")]
[JsonSerializable(typeof(List<Stat>), TypeInfoPropertyName = "InternalListStat")]
[JsonSerializable(typeof(StatTier))]
[JsonSerializable(typeof(Mod))]
[JsonSerializable(typeof(ModStat))]
public partial class DomainJsonContext : JsonSerializerContext
{
}