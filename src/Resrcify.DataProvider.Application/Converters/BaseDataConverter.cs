using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Domain.Internal.BaseData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.CrTable;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.DatacronData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GearData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.GpTable;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.ModeSetData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.RelicData;
using Resrcify.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;
namespace Resrcify.DataProvider.Application.Converters;
internal sealed class BaseDataConverter : JsonConverter<BaseData>
{
    public override BaseData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        var id = root.GetProperty("id").GetGuid();

        var gear = JsonSerializer.Deserialize<Dictionary<string, GearData>>(root.GetProperty("gear").GetRawText(), options);
        var modSets = JsonSerializer.Deserialize<Dictionary<string, ModSetData>>(root.GetProperty("modSets").GetRawText(), options);
        var crTable = JsonSerializer.Deserialize<CrTable>(root.GetProperty("crTable").GetRawText(), options);
        var gpTable = JsonSerializer.Deserialize<GpTable>(root.GetProperty("gpTable").GetRawText(), options);
        var relics = JsonSerializer.Deserialize<Dictionary<string, RelicData>>(root.GetProperty("relics").GetRawText(), options);
        var units = JsonSerializer.Deserialize<Dictionary<string, UnitData>>(root.GetProperty("units").GetRawText(), options);
        var datacrons = JsonSerializer.Deserialize<Dictionary<string, DatacronData>>(root.GetProperty("datacrons").GetRawText(), options);

        return BaseData.Create(
            id, gear ?? [],
            modSets ?? [],
            crTable ?? CrTable.Create([], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []).Value,
            gpTable ?? GpTable.Create([], [], [], [], [], [], [], [], [], [], [], [], [], []).Value,
            relics ?? [],
            units ?? [],
            datacrons ?? []);
    }
    public override void Write(Utf8JsonWriter writer, BaseData value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id.ToString());
        writer.WriteStartObjectProperty("gear", value.Gear, options);
        writer.WriteStartObjectProperty("modSets", value.ModSets, options);
        writer.WriteStartObjectProperty("crTable", value.CrTable, options);
        writer.WriteStartObjectProperty("gpTable", value.GpTable, options);
        writer.WriteStartObjectProperty("relics", value.Relics, options);
        writer.WriteStartObjectProperty("units", value.Units, options);
        writer.WriteStartObjectProperty("datacrons", value.Datacrons, options);
        writer.WriteEndObject();
    }
}