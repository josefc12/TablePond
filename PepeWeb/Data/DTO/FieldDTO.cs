using PepeWeb.Data.Enums;
using PepeWeb.Data.Models;

namespace PepeWeb.Data.DTO
{
    public class FieldDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TableDTO Table { get; set; }
        public CustomFieldType Type { get; set; }
    }
}
