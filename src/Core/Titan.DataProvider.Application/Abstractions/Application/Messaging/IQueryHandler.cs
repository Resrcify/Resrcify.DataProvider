using MediatR;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Application.Abstractions.Application.Messaging;


public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
