namespace Minimizer.Common.Models.Reports
{
    public class ReportResponse
    {
        public Guid LeadId { get; set; }
        public string LeadName { get; set; }
        public string LeadEmail { get; set; }
        public bool ConvertedToOpportunity { get; set; }
        public Guid? OpportunityId { get; set; }
        public decimal OpportunityValue { get; set; }
        public string OpportunityStage { get; set; }
    }
}
