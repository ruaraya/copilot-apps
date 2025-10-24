namespace Minimizer.Entities.Models
{
    public class Opportunity
    {
        public Guid Id { get; set; }
        public Guid LeadId { get; set; }
        public decimal Value { get; set; }
        public string Stage { get; set; }
        public DateTime CreatedAt { get; set; }
        public Lead Lead { get; set; }
    }
}
