using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Resrcify.DataProvider.Application.Extensions;
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
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        Guid id = Guid.Empty;
        Dictionary<string, GearData>? gear = null;
        Dictionary<string, ModSetData>? modSets = null;
        CrTable? crTable = null;
        GpTable? gpTable = null;
        Dictionary<string, RelicData>? relics = null;
        Dictionary<string, UnitData>? units = null;
        Dictionary<string, DatacronData>? datacrons = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propName = reader.GetString()!;
            reader.Read(); // move to value

            switch (propName)
            {
                case "id":
                    if (reader.TokenType != JsonTokenType.String)
                        throw new JsonException();
                    id = reader.GetGuid();
                    break;

                case "gear":
                    gear = JsonSerializer.Deserialize<Dictionary<string, GearData>>(ref reader, options);
                    break;

                case "modSets":
                    modSets = JsonSerializer.Deserialize<Dictionary<string, ModSetData>>(ref reader, options);
                    break;

                case "crTable":
                    crTable = JsonSerializer.Deserialize<CrTable>(ref reader, options);
                    break;

                case "gpTable":
                    gpTable = JsonSerializer.Deserialize<GpTable>(ref reader, options);
                    break;

                case "relics":
                    relics = JsonSerializer.Deserialize<Dictionary<string, RelicData>>(ref reader, options);
                    break;

                case "units":
                    units = JsonSerializer.Deserialize<Dictionary<string, UnitData>>(ref reader, options);
                    break;

                case "datacrons":
                    datacrons = JsonSerializer.Deserialize<Dictionary<string, DatacronData>>(ref reader, options);
                    break;

                default:
                    reader.Skip();
                    break;
            }
        }

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