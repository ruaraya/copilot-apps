using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Opportunities;
using Minimizer.Common.Models.Reports;
using Minimizer.Data.DbContexts;

namespace Minimizer.Services.Reports
{
    public class ReportQueryService :
        IRequestHandler<GetLeadConversionReportQuery, List<ReportResponse>>
    {
        private readonly MinimizerDbContext context;

        public ReportQueryService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<List<ReportResponse>> Handle(GetLeadConversionReportQuery request,
            CancellationToken cancellationToken) =>

            await GetLeadConversionReportAsync(cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<List<ReportResponse>> GetLeadConversionReportAsync(CancellationToken cancellationToken)
        {
            var leads = await context.Leads.ToListAsync(cancellationToken);
            var opportunities = await context.Opportunities.ToListAsync(cancellationToken);

            var report = leads.Select(lead =>
            {
                var opportunity = opportunities.FirstOrDefault(o => o.LeadId == lead.Id);
                return new ReportResponse
                {
                    LeadId = lead.Id,
                    LeadName = lead.Name,
                    LeadEmail = lead.Email,
                    ConvertedToOpportunity = opportunity != null,
                    OpportunityId = opportunity?.Id ?? null,
                    OpportunityValue = opportunity?.Value ?? 0,
                    OpportunityStage = opportunity?.Stage ?? string.Empty
                };
            }).ToList();

            return report;
        }

        #endregion Public methods
    }

    #region Queries
    public record GetLeadConversionReportQuery() : IRequest<List<ReportResponse>>;

    #endregion Queries
}