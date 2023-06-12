using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json.Serialization;

namespace Titan.DataProvider.Application.Resolvers;

/// <summary>
/// Provides an enhanced contract resolver for <see cref="JsonConvert"/> which
/// supports constructors with custom attributes and private constructors.
/// </summary>
/// <remarks>
/// Partially based on https://stackoverflow.com/a/35865022.
/// </remarks>
public class CustomConstructorResolver : DefaultContractResolver
{
    /// <summary>
    /// Gets or sets the name of the attribute that marks the constructor to be used
    /// for deserialization.
    /// </summary>
    public string ConstructorAttributeName { get; set; } = "JsonConstructorAttribute";

    /// <summary>
    /// Gets or sets a value indicating whether to ignore custom attributes when
    /// looking for constructors for deserializing types.
    /// </summary>
    /// <value>
    /// <c>true</c> if custom attributes on constructors should be ignored,
    /// <c>false</c> if a single constructor marked with an attribute named
    /// <see cref="ConstructorAttributeName"/> should be used for deserialization.
    /// The default value is <c>false</c>.
    /// </value>
    public bool IgnoreAttributeConstructor { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to ignore private constructors
    /// when looking for constructors for deserializing types.
    /// </summary>
    /// <value>
    /// <c>true</c> if private constructors should be ignored,
    /// <c>false</c> if a single private constructor should be used for deserialization.
    /// The default value is <c>false</c>.
    /// </value>
    public bool IgnoreSinglePrivateConstructor { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to ignore the number of parameters
    /// when looking for constructors for deserializing types.
    /// </summary>
    /// <value>
    /// <c>true</c> if the number of parameters should be ignored,
    /// <c>false</c> if the constructor with the greatest number of parameters
    /// should be used for deserialization.
    /// The default value is <c>false</c>.
    /// </value>
    public bool IgnoreMostSpecificConstructor { get; set; } = false;

    /// <summary>
    /// Creates a <see cref="JsonObjectContract" /> for the given type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>A <see cref="JsonObjectContract" /> for the given type.</returns>
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);

        // Use default contract for non-object types.
        if (objectType.IsPrimitive || objectType.IsEnum) return contract;

        // Look for constructor with attribute first, then single private, then most specific.
        var overrideConstructor =
               (this.IgnoreAttributeConstructor ? null : GetAttributeConstructor(objectType))
            ?? (this.IgnoreSinglePrivateConstructor ? null : GetSinglePrivateConstructor(objectType))
            ?? (this.IgnoreMostSpecificConstructor ? null : GetMostSpecificConstructor(objectType));

        // Set override constructor if found, otherwise use default contract.
        if (overrideConstructor != null)
        {
            SetOverrideCreator(contract, overrideConstructor);
        }

        return contract;
    }

    private void SetOverrideCreator(JsonObjectContract contract, ConstructorInfo attributeConstructor)
    {
        contract.OverrideCreator = CreateParameterizedConstructor(attributeConstructor);
        contract.CreatorParameters.Clear();
        foreach (var constructorParameter in base.CreateConstructorParameters(attributeConstructor, contract.Properties))
        {
            contract.CreatorParameters.Add(constructorParameter);
        }
    }

    private static ObjectConstructor<object>? CreateParameterizedConstructor(MethodBase method)
    {
        var c = method as ConstructorInfo;
        if (c != null)
            return a => c.Invoke(a);
        return a => method.Invoke(null, a)!;
    }

    /// <summary>
    /// Returns the single constructor marked with a <see cref="ConstructorAttributeName"/>
    /// for <paramref name="objectType"/> if defined, <c>null</c> otherwise.
    /// </summary>
    /// <exception cref="JsonException">More than one constructor is marked with a
    /// <see cref="ConstructorAttributeName"/>.</exception>
    protected virtual ConstructorInfo? GetAttributeConstructor(Type objectType)
    {
        var constructors = objectType
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(c => c.GetCustomAttributes().Any(a => a.GetType().Name == this.ConstructorAttributeName)).ToList();

        if (constructors.Count == 1) return constructors[0];
        if (constructors.Count > 1)
            throw new JsonException($"Multiple constructors with a {this.ConstructorAttributeName}.");

        return null;
    }

    /// <summary>
    /// Returns the single non-public constructor for <paramref name="objectType"/>
    /// if defined, <c>null</c> otherwise.
    /// </summary>
    protected virtual ConstructorInfo? GetSinglePrivateConstructor(Type objectType)
    {
        var constructors = objectType
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

        return constructors.Length == 1 ? constructors[0] : null;
    }

    /// <summary>
    /// Returns the constructor with the greatest number of parameters for
    /// <paramref name="objectType"/>.
    /// </summary>
    protected virtual ConstructorInfo? GetMostSpecificConstructor(Type objectType)
    {
        var constructors = objectType
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .OrderBy(e => e.GetParameters().Length);

        var mostSpecific = constructors.LastOrDefault();
        return mostSpecific;
    }
}
