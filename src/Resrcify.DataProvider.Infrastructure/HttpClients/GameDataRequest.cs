namespace Resrcify.DataProvider.Infrastructure.HttpClients;

internal class GameDataRequest
{
    public string? Version { get; set; }

    public string? DevicePlatform { get; set; }

    public bool IncludePveUnits { get; set; }

    public GameDataSegment RequestSegment { get; set; }

    public long Items { get; set; }
}
internal enum GameDataSegment
{
    Gamedatasegmentall,
    Gamedatasegment1000,
    Gamedatasegment2000,
    Gamedatasegment3000,
    Gamedatasegment4000
}
