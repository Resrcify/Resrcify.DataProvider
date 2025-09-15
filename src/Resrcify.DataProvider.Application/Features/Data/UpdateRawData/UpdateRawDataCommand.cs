using Resrcify.DataProvider.Application.Models;
using Resrcify.SharedKernel.Messaging.Abstractions;

namespace Resrcify.DataProvider.Application.Features.Data.UpdateRawData;

public sealed record UpdateRawDataCommand(MetadataResponse MetadataResponse)
    : ICommand;
