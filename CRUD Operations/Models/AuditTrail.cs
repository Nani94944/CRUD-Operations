namespace CRUD_Operations.Data
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; }
        public string Changes { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}