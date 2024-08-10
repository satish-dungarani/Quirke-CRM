namespace Quirke.CRM.Domain
{
    public class Master
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MasterTypeId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
