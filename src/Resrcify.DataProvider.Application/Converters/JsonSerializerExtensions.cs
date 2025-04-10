using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;

namespace Resrcify.DataProvider.Application.Converters;

public static class JsonSerializerExtensions
{
    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
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
public class UnitConverter : JsonConverter<Unit>
{
    public override Unit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Read the entire JSON object into a JsonDocument
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        // Deserialize the properties of Unit
        var baseId = root.GetProperty("baseId").GetString();
        var nameKey = root.GetProperty("nameKey").GetString();
        var combatType = root.GetProperty("combatType").GetInt32();

        // Use the Unit.Create method to create the Unit instance
        return Unit.Create(
            baseId ?? string.Empty,
            nameKey ?? string.Empty,
            combatType).Value;
    }

    public override void Write(Utf8JsonWriter writer, Unit value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Serialize the properties of Unit
        writer.WriteString("baseId", value.BaseId);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteNumber("combatType", value.CombatType);

        writer.WriteEndObject();
    }
}