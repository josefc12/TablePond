using System.ComponentModel.DataAnnotations;

namespace PepeWeb.Data.Models
{
    public class Value
    {
        [Key]
        public int Id { get; set; }
        public string Val { get; set; }
        public int TableId { get; set; }
        public int FieldId { get; set; }
        public int ItemId { get; set; }

    }
}
