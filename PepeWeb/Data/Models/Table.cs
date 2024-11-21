using System.ComponentModel.DataAnnotations;

namespace TablePond.Data.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public int ItemAmount { get; set; }
    }
}
