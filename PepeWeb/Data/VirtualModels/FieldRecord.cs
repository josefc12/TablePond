using TablePond.Data.DTO;
using TablePond.Data.Enums;
using System.Globalization;

namespace TablePond.Data.VirtualModels
{
    public class FieldRecord
    {
        public int ItemId { get; set; }
        public string? Name { get; set; }
        public CustomFieldType Type { get; set; }
        public string? TextValue { get; set; }
        public int? NumberValue { get; set; }
        public FieldDTO? Field { get; set; }
    }
}
