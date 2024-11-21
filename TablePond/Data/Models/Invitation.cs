using System.ComponentModel.DataAnnotations;

namespace TablePond.Data.Models
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public required string Code { get; set; }
        public required string EmailAddress { get; set; }
        public bool IsActive { get; set; }
    }
}
