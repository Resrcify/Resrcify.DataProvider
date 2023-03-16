using System.Collections.Generic;

namespace Titan.DataProvider.Application.Models.GalaxyOfHeroes.GameData
{
    public class Table
    {
        public string? Id { get; set; }
        public List<TableRow> Row { get; set; } = new();
    }
}
