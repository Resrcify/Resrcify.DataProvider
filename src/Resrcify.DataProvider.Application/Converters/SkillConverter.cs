using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class SkillConverter : JsonConverter<Skill>
{
    public override Skill Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var id = root.GetProperty("id").GetString();
        var name = root.GetProperty("name").GetString();
        var nameKey = root.GetProperty("nameKey").GetString();
        var maxTier = root.GetProperty("maxTier").GetInt32();
        var type = root.GetProperty("type").GetInt64();
        var image = root.GetProperty("image").GetString();
        var powerOverrideTags = JsonSerializer.Deserialize<Dictionary<string, string>>(root.GetProperty("powerOverrideTags").GetRawText(), options);
        var isZeta = root.GetProperty("isZeta").GetBoolean();
        var zetaTier = root.GetProperty("zetaTier").GetInt32();
        var isOmicron = root.GetProperty("isOmicron").GetBoolean();
        var omicronTier = root.GetProperty("omicronTier").GetInt32();
        var omicronMode = JsonSerializer.Deserialize<OmicronMode>(root.GetProperty("omicronMode").GetRawText(), options);
        var omicronModeName = root.GetProperty("omicronModeName").GetString();

        return Skill.Create(
            id ?? string.Empty,
            name ?? string.Empty,
            nameKey ?? string.Empty,
            maxTier,
            type,
            image ?? string.Empty,
            powerOverrideTags ?? [],
            isZeta,
            zetaTier,
            isOmicron,
            omicronTier,
            omicronMode,
            omicronModeName ?? string.Empty).Value;
    }

    public override void Write(Utf8JsonWriter writer, Skill value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("name", value.Name);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteNumber("maxTier", value.MaxTier);
        writer.WriteNumber("type", value.Type);
        writer.WriteString("image", value.Image);
        writer.WriteStartObjectProperty("powerOverrideTags", value.PowerOverrideTags, options);
        writer.WriteBoolean("isZeta", value.IsZeta);
        writer.WriteNumber("zetaTier", value.ZetaTier);
        writer.WriteBoolean("isOmicron", value.IsOmicron);
        writer.WriteNumber("omicronTier", value.OmicronTier);
        writer.WriteStartObjectProperty("omicronMode", value.OmicronMode, options);
        writer.WriteString("omicronModeName", value.OmicronModeName);
        writer.WriteEndObject();
    }

}
