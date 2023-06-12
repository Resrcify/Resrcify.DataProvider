using MediatR;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Application.Abstractions.Application.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
