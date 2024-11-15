using PepeWeb.Data.Models;
using PepeWeb.Data.DTO;

namespace PepeWeb.Data.VirtualModels
{
    public class IntermediateNewTableData
    {
        public string? TableName { get; set; }
        public List<FieldDTO>? Fields { get; set; }
        public string? UserId { get; set; }

    }
}
