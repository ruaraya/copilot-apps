using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Opportunities;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Opportunities
{
    public class OpportunityQueryService :
        IRequestHandler<CheckOpportunityByOpportunityIdQuery, bool>,
        IRequestHandler<GetAllOpportunitiesQuery, List<OpportunityResponse>>,
        IRequestHandler<GetOpportunityByOpportunityIdQuery, OpportunityResponse?>
    {
        private readonly MinimizerDbContext context;

        public OpportunityQueryService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<bool> Handle(CheckOpportunityByOpportunityIdQuery request, CancellationToken cancellationToken) =>
            await context.Opportunities.AnyAsync(x => x.Id == request.OpportunityId, cancellationToken);

        public async Task<List<OpportunityResponse>> Handle(GetAllOpportunitiesQuery request,
            CancellationToken cancellationToken) =>

            await GetAllOpportunitiesAsync(cancellationToken);
       

        public async Task<OpportunityResponse?> Handle(GetOpportunityByOpportunityIdQuery request,
            CancellationToken cancellationToken) =>

            await GetOpportunityByOpportunityIdAsync(request.OpportunityId, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<List<OpportunityResponse>> GetAllOpportunitiesAsync(CancellationToken cancellationToken)
        {
            var opportunityEntities = await GetAllOpportunityEntitiesAsync(cancellationToken);

            if (!opportunityEntities.Any())
                return new List<OpportunityResponse>();

            var opportunities = opportunityEntities.Select(o => new OpportunityResponse
            {
                LeadId = o.LeadId,
                Value = o.Value,
                Stage = o.Stage,
                CreatedAt = o.CreatedAt
            }).ToList();

            return opportunities;
        }

        public async Task<OpportunityResponse?> GetOpportunityByOpportunityIdAsync(Guid opportunityId, CancellationToken cancellationToken)
        {
            var opportunityEntity = await GetOpportunityEntityByOpportunityIdAsync(opportunityId, cancellationToken);

            if (opportunityEntity == null)
                return null;

            return new OpportunityResponse
            {
                LeadId = opportunityEntity.LeadId,
                Value = opportunityEntity.Value,
                Stage = opportunityEntity.Stage,
                CreatedAt = opportunityEntity.CreatedAt
            };
        }

        #endregion Public methods

        #region Private methods

        private async Task<List<Opportunity>> GetAllOpportunityEntitiesAsync(CancellationToken cancellationToken)
        {
            return await context.Opportunities.ToListAsync(cancellationToken);
        }

        private async Task<Opportunity?> GetOpportunityEntityByOpportunityIdAsync(Guid opportunityId, CancellationToken cancellationToken)
        {
            return await context.Opportunities.FirstOrDefaultAsync(x => x.Id == opportunityId, cancellationToken);
        }

        #endregion Private methods

    }

    #region Queries

    public record CheckOpportunityByOpportunityIdQuery(Guid OpportunityId) : IRequest<bool>;
    public record GetAllOpportunitiesQuery() : IRequest<List<OpportunityResponse>>;
    public record GetOpportunityByOpportunityIdQuery(Guid OpportunityId) : IRequest<OpportunityResponse?>;

    #endregion Queries
}