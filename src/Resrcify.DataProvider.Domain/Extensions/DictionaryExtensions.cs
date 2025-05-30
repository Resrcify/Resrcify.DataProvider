using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Extensions;

public static class DictionaryExtensions
{
    public static double GetOrDefault<TKey>(this IDictionary<TKey, double> dict, TKey key, double defaultValue = 0)
    {
        return dict.TryGetValue(key, out var existing) ? existing : defaultValue;
    }
}
