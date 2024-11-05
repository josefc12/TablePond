using PepeWeb.Data.Enums;

namespace PepeWeb.Data.DTO
{
    public class FieldDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TableId { get; set; }
        public CustomFieldType Type { get; set; }
    }
}
