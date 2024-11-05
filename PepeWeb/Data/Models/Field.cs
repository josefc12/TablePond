using Microsoft.VisualBasic.FileIO;
using PepeWeb.Data.Enums;

namespace PepeWeb.Data.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TableId { get; set; }
        public CustomFieldType Type { get; set; }
    }
}
