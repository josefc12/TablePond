using System.ComponentModel.DataAnnotations;

namespace PepeWeb.Data.Models
{
    public class Value
    {
        [Key]
        public int Id { get; set; }
        public string? Val { get; set; }
        public Table? Table { get; set; }
        public Field? Field { get; set; }
        public int ItemId { get; set; }

    }
}
