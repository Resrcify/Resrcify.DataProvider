using Resrcify.SharedKernel.Messaging.Abstractions;
using Resrcify.DataProvider.Application.Models.GalaxyOfHeroes.Metadata;

namespace Resrcify.DataProvider.Application.Features.Data.GetMetadataVersion;

public sealed record GetMetadataVersionQuery()
    : IQuery<MetadataResponse>;
