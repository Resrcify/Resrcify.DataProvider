using Titan.DataProvider.Application.Abstractions.Application.Messaging;
using Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData;

namespace Titan.DataProvider.Application.Features.Data.Queries.GetCachedGameData
{
    public sealed record GetCachedGameDataQuery() : IQuery<GameDataResponse>
    {
    }
}