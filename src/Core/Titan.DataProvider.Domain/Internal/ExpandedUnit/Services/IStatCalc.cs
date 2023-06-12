using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Internal.ExpandedUnit.Services
{
    public interface IStatCalc
    {
        IReadOnlyDictionary<long, double> Base { get; }
        IReadOnlyDictionary<long, double> Gear { get; }
        IReadOnlyDictionary<long, double> Mods { get; }
        IReadOnlyDictionary<long, double> Crew { get; }
        double Gp { get; }
    }
}