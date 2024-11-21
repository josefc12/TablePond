using System.ComponentModel.DataAnnotations;

namespace PepeWeb.Data.DTO
{
    public class InvitationDTO
    {
        public int Id { get; set; }
        public required string CreatedBy { get; set; }
        public required string Code { get; set; }
        public required string EmailAddress { get; set; }
        public bool IsActive { get; set; }
    }
}
