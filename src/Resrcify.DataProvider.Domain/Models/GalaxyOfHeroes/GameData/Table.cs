using System.Collections.Generic;

namespace Resrcify.DataProvider.Domain.Models.GalaxyOfHeroes.GameData;

public class Table
{
    public string? Id { get; set; }
    public List<TableRow> Rows { get; set; } = [];
}
