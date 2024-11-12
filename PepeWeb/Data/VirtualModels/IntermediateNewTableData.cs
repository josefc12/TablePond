using PepeWeb.Data.Models;

namespace PepeWeb.Data.VirtualModels
{
    public class IntermediateNewTableData
    {
        public string TableName { get; set; }
        public List<Field> Fields { get; set; }
        public string UserId { get; set; }

    }
}
