using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Resrcify.DataProvider.Application.Resolvers;

public class CustomConstructorConverterFactory : JsonConverterFactory
{
    public string ConstructorAttributeName { get; set; } = "JsonConstructorAttribute";
    public bool IgnoreAttributeConstructor { get; set; }
    public bool IgnoreSinglePrivateConstructor { get; set; }
    public bool IgnoreMostSpecificConstructor { get; set; }

    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsClass;

    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        var ctor = GetConstructor(type) ?? throw new InvalidOperationException($"No suitable constructor found for {type}");

        var converterType = typeof(CustomConstructorConverter<>).MakeGenericType(type);
        return (JsonConverter)Activator.CreateInstance(converterType, ctor)!;
    }

    private ConstructorInfo? GetConstructor(Type type)
    {
        if (!IgnoreAttributeConstructor)
        {
            var attrCtor = type
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetCustomAttributes().Any(a => a.GetType().Name == ConstructorAttributeName));
            if (attrCtor != null)
                return attrCtor;
        }

        if (!IgnoreSinglePrivateConstructor)
        {
            var privCtors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            if (privCtors.Length == 1)
                return privCtors[0];
        }

        if (!IgnoreMostSpecificConstructor)
        {
            return type
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();
        }

        return null;
    }
}

public class CustomConstructorConverter<T> : JsonConverter<T>
{
    private readonly ConstructorInfo _constructor;
    private readonly ParameterInfo[] _parameters;

    public CustomConstructorConverter(ConstructorInfo constructor)
    {
        _constructor = constructor;
        _parameters = constructor.GetParameters();
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var args = new object?[_parameters.Length];

        for (int i = 0; i < _parameters.Length; i++)
        {
            var param = _parameters[i];
            args[i] = root.TryGetProperty(param.Name!, out var prop)
                ? JsonSerializer.Deserialize(prop.GetRawText(), param.ParameterType, options)
                : param.HasDefaultValue ? param.DefaultValue : GetDefault(param.ParameterType);
        }

        return (T)_constructor.Invoke(args);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }

    private static object? GetDefault(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;
}