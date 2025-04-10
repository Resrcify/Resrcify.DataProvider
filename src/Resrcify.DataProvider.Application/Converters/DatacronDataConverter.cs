using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;
using Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

namespace Resrcify.DataProvider.Application.Converters;

internal sealed class DatacronDataConverter : JsonConverter<DatacronData>
{
    public override DatacronData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var id = root.GetProperty("id").GetString();
        var setId = root.GetProperty("setId").GetInt32();
        var nameKey = root.GetProperty("nameKey").GetString();
        var iconKey = root.GetProperty("iconKey").GetString();
        var detailPrefab = root.GetProperty("detailPrefab").GetString();
        var expirationTimeMs = root.GetProperty("expirationTimeMs").GetInt64();
        var allowReroll = root.GetProperty("allowReroll").GetBoolean();
        var initialTiers = root.GetProperty("initialTiers").GetInt32();
        var maxRerolls = root.GetProperty("maxRerolls").GetInt32();
        var referenceTemplateId = root.GetProperty("referenceTemplateId").GetString();

        var setMaterial = JsonSerializer.Deserialize<List<DatacronSetMaterial>>(root.GetProperty("setMaterial").GetRawText(), options);
        var fixedTag = JsonSerializer.Deserialize<List<string>>(root.GetProperty("fixedTag").GetRawText(), options);
        var setTier = JsonSerializer.Deserialize<List<DatacronSetTier>>(root.GetProperty("setTier").GetRawText(), options);
        var tier = JsonSerializer.Deserialize<List<DatacronTemplateTier>>(root.GetProperty("tier").GetRawText(), options);
        var affixSet = JsonSerializer.Deserialize<List<DatacronAffixTemplateSet>>(root.GetProperty("affixSet").GetRawText(), options);
        var abilities = JsonSerializer.Deserialize<Dictionary<string, Domain.Internal.BaseData.ValueObjects.DatacronData.Ability>>(root.GetProperty("abilities").GetRawText(), options);
        var stats = JsonSerializer.Deserialize<Dictionary<string, Stat>>(root.GetProperty("stats").GetRawText(), options);

        return DatacronData.Create(
            id ?? string.Empty,
            setId,
            nameKey ?? string.Empty,
            iconKey ?? string.Empty,
            detailPrefab ?? string.Empty,
            expirationTimeMs,
            allowReroll,
            initialTiers,
            maxRerolls,
            referenceTemplateId ?? string.Empty,
            setMaterial ?? [],
            fixedTag ?? [],
            setTier ?? [],
            tier ?? [],
            affixSet ?? [],
            abilities ?? [],
            stats ?? []).Value;
    }

    public override void Write(Utf8JsonWriter writer, DatacronData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", value.Id);
        writer.WriteNumber("setId", value.SetId);
        writer.WriteString("nameKey", value.NameKey);
        writer.WriteString("iconKey", value.IconKey);
        writer.WriteString("detailPrefab", value.DetailPrefab);
        writer.WriteNumber("expirationTimeMs", value.ExpirationTimeMs);
        writer.WriteBoolean("allowReroll", value.AllowReroll);
        writer.WriteNumber("initialTiers", value.InitialTiers);
        writer.WriteNumber("maxRerolls", value.MaxRerolls);
        writer.WriteString("referenceTemplateId", value.ReferenceTemplateId);

        writer.WriteStartObjectProperty("setMaterial", value.SetMaterial, options);
        writer.WriteStartObjectProperty("fixedTag", value.FixedTag, options);
        writer.WriteStartObjectProperty("setTier", value.SetTier, options);
        writer.WriteStartObjectProperty("tier", value.Tier, options);
        writer.WriteStartObjectProperty("affixSet", value.AffixSet, options);
        writer.WriteStartObjectProperty("abilities", value.Abilities, options);
        writer.WriteStartObjectProperty("stats", value.Stats, options);

        writer.WriteEndObject();
    }
}
