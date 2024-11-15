using PepeWeb.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PepeWeb.Data.Models
{
    public class Field
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Table? Table { get; set; }
        public CustomFieldType Type { get; set; }
    }
}
