namespace PepeWeb.Data.DTO
{
    public class ValueDTO
    {
        public int Id { get; set; }
        public string Val { get; set; }
        public TableDTO Table { get; set; }
        public FieldDTO Field { get; set; }
        public int ItemId { get; set; }
    }
}
