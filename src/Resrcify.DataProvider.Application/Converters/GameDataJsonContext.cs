using System.Collections.Generic;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.CrTable;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GearData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GpTable;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.ModeSetData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.RelicData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;
using Ability = Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData.Ability;

namespace Resrcify.DataProvider.Application.Converters;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(BaseData))]
[JsonSerializable(typeof(GearData))]
[JsonSerializable(typeof(ModSetData))]
[JsonSerializable(typeof(CrTable))]
[JsonSerializable(typeof(GpTable))]
[JsonSerializable(typeof(RelicData))]
[JsonSerializable(typeof(UnitData))]
[JsonSerializable(typeof(GearLevel))]
[JsonSerializable(typeof(Skill))]
[JsonSerializable(typeof(ModRecommendation))]
[JsonSerializable(typeof(DatacronData))]
[JsonSerializable(typeof(Ability))]
[JsonSerializable(typeof(Target))]
[JsonSerializable(typeof(Unit))]
[JsonSerializable(typeof(Stat))]
[JsonSerializable(typeof(Dictionary<string, GearData>))]
[JsonSerializable(typeof(Dictionary<string, GearLevel>))]
[JsonSerializable(typeof(Dictionary<string, ModSetData>))]
[JsonSerializable(typeof(Dictionary<string, Target>))]
[JsonSerializable(typeof(Dictionary<string, RelicData>))]
[JsonSerializable(typeof(Dictionary<string, UnitData>))]
[JsonSerializable(typeof(Dictionary<string, DatacronData>))]
[JsonSerializable(typeof(List<DatacronSetMaterial>))]
[JsonSerializable(typeof(List<DatacronSetTier>))]
[JsonSerializable(typeof(List<DatacronTemplateTier>))]
[JsonSerializable(typeof(List<DatacronAffixTemplateSet>))]
[JsonSerializable(typeof(Dictionary<string, Ability>))]
[JsonSerializable(typeof(Dictionary<string, Stat>))]
[JsonSerializable(typeof(List<Skill>))]
[JsonSerializable(typeof(List<ModRecommendation>))]
[JsonSerializable(typeof(Dictionary<string, double>))]
[JsonSerializable(typeof(Dictionary<long, long>))]
[JsonSerializable(typeof(Dictionary<string, long>))]
public partial class GameDataJsonContext : JsonSerializerContext
{
}