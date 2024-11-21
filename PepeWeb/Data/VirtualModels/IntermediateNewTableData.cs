using TablePond.Data.Models;
using TablePond.Data.DTO;

namespace TablePond.Data.VirtualModels
{
    public class IntermediateNewTableData
    {
        public string? TableName { get; set; }
        public List<FieldDTO>? Fields { get; set; }
        public string? UserId { get; set; }

    }
}
