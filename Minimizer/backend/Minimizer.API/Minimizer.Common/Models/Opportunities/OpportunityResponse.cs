namespace Minimizer.Common.Models.Opportunities
{
    public class OpportunityResponse
    {
        public Guid LeadId { get; set; }
        public decimal Value { get; set; }
        public string Stage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}