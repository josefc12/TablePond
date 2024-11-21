using PepeWeb.Data.VirtualModels;

namespace PepeWeb.Data.DTO
{
    public class TableConstructDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<FieldDTO> Columns { get; set; } = new List<FieldDTO>();
        public List<TableRow> Rows {get; set;} = new List<TableRow>();
    }
}
