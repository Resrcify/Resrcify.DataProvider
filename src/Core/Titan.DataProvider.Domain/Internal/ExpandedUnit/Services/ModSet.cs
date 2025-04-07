using System.Collections.Generic;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;
using Titan.DataProvider.Domain.Internal.ExpandedUnit.Enums;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.Services;

public sealed class ModSet : ValueObject
{
    private const int MODMAXLEVEL = 15;
    public ModType ModType { get; private set; }
    public int Count { get; private set; }
    public int MaxLevelCount { get; private set; }

    private ModSet(ModType modType, int maxLevelCount, int count = 1)
    {
        ModType = modType;
        Count = count;
        MaxLevelCount = maxLevelCount;
    }

    public static Result<ModSet> Create(ModType modType, int modLevel)
    {
        var isMaxLevel = modLevel == MODMAXLEVEL;
        var maxLevelCount = isMaxLevel ? 1 : 0;
        return new ModSet(modType, maxLevelCount);
    }

    public Result AddMod(int modLevel)
    {
        Count++;
        var isMaxLevel = modLevel == MODMAXLEVEL;
        if (isMaxLevel)
            MaxLevelCount++;
        return Result.Success();
    }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return ModType;
        yield return Count;
        yield return MaxLevelCount;
    }
}