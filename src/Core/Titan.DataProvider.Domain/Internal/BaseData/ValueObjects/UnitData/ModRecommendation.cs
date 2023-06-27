using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects.UnitData;

public sealed class ModRecommendation : ValueObject
{
    public string RecommendationSetId { get; private set; }
    public long UnitTier { get; private set; }

    private ModRecommendation(string recommendationSetId, long unitTier)
    {
        RecommendationSetId = recommendationSetId;
        UnitTier = unitTier;
    }

    public static Result<ModRecommendation> Create(string recommendationSetId, long unitTier)
    {
        return new ModRecommendation(recommendationSetId, unitTier);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return RecommendationSetId;
        yield return UnitTier;
    }
}