namespace Minimizer.Common.Models.Opportunities
{
    public class UpsertOpportunityRequest
    {
        public Guid LeadId { get; set; }
        public decimal Value { get; set; }
        public string Stage { get; set; }
    }
}