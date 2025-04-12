using System.Text.Json;
using Resrcify.DataProvider.Application.Converters;

namespace Resrcify.DataProvider.Application.Extensions;

public static class JsonSerializerExtensions
{
    public static JsonSerializerOptions GetDomainSerializerOptions()
    {
        var options = new JsonSerializerOptions(GameDataJsonContext.Default.Options);

        options.Converters.Add(new BaseDataConverter());
        options.Converters.Add(new GearDataConverter());
        options.Converters.Add(new ModSetDataConverter());
        options.Converters.Add(new CrTableConverter());
        options.Converters.Add(new GpTableConverter());
        options.Converters.Add(new RelicDataConverter());
        options.Converters.Add(new UnitDataConverter());
        options.Converters.Add(new GearLevelConverter());
        options.Converters.Add(new SkillConverter());
        options.Converters.Add(new ModRecommendationConverter());
        options.Converters.Add(new DatacronDataConverter());
        options.Converters.Add(new AbilityConverter());
        options.Converters.Add(new TargetConverter());
        options.Converters.Add(new UnitConverter());
        options.Converters.Add(new StatConverter());

        return options;
    }

    public static void WriteStartObjectProperty(this Utf8JsonWriter writer, string propertyName, object value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(propertyName);
        JsonSerializer.Serialize(writer, value, options);
    }
}
