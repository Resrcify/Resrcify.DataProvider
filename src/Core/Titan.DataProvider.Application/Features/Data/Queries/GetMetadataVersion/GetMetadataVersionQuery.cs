using Resrcify.SharedKernel.Messaging.Abstractions;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetMetadataVersion;

public sealed record GetMetadataVersionQuery()
    : IQuery<MetadataResponse>;
