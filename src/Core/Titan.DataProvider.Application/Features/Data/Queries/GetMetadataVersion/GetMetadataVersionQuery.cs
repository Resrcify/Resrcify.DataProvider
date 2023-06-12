using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetMetadataVersion;

public sealed record GetMetadataVersionQuery() : IQuery<MetadataResponse>
{
}
