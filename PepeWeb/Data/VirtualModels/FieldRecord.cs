using PepeWeb.Data.DTO;
using PepeWeb.Data.Enums;
using System.Globalization;

namespace PepeWeb.Data.VirtualModels
{
    public class FieldRecord
    {
        public string Name { get; set; }
        public CustomFieldType Type { get; set; }
        public string? TextValue { get; set; }
        public int? NumberValue { get; set; }
        public FieldDTO Field { get; set; }
    }
}
