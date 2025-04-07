using System.Collections.Generic;

namespace Titan.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class XpTable
{
    public string? Id { get; set; }
    public List<XpTableRow> Rows { get; set; } = [];

}
