using System.Collections.Generic;
using Resrcify.SharedKernel.DomainDrivenDesign.Primitives;
using Resrcify.SharedKernel.ResultFramework.Primitives;

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
        => new ModRecommendation(recommendationSetId, unitTier);

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return RecommendationSetId;
        yield return UnitTier;
    }
}