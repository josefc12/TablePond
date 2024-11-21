using TablePond.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace TablePond.Data.Models
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
