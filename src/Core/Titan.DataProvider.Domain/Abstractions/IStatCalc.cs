using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Abstractions;

public interface IStatCalc
{
    IReadOnlyDictionary<int, double> Base { get; }
    IReadOnlyDictionary<int, double> Gear { get; }
    IReadOnlyDictionary<int, double> Mods { get; }
    IReadOnlyDictionary<int, double> Crew { get; }
    double Gp { get; }
    double CrewGp { get; }
}