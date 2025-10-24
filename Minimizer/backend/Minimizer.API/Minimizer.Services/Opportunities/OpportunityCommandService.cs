using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Opportunities;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Opportunities
{
    public class OpportunityCommandService :
        IRequestHandler<InsertOpportunityCommand, OpportunityResponse>,
        IRequestHandler<UpdateOpportunityCommand, OpportunityResponse>,
        IRequestHandler<DeleteOpportunityCommand, Unit>
    {
        private readonly MinimizerDbContext context;

        public OpportunityCommandService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<OpportunityResponse> Handle(InsertOpportunityCommand request,
            CancellationToken cancellationToken) =>

            await InsertOpportunityAsync(request.UpsertOpportunityRequest, cancellationToken);        

        public async Task<OpportunityResponse> Handle(UpdateOpportunityCommand request,
            CancellationToken cancellationToken) =>

            await UpdateOpportunityAsync(request.OpportunityId, request.UpsertOpportunityRequest, cancellationToken);

        public async Task<Unit> Handle(DeleteOpportunityCommand request,
            CancellationToken cancellationToken) =>

            await DeleteOpportunityAsync(request.OpportunityId, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<OpportunityResponse> InsertOpportunityAsync(UpsertOpportunityRequest request, CancellationToken cancellationToken)
        {
            Opportunity newOpportunity = await InsertOpportunityEntityAsync(request, cancellationToken);

            return new OpportunityResponse
            {
                LeadId = newOpportunity.LeadId,
                Value = newOpportunity.Value,
                Stage = newOpportunity.Stage,
                CreatedAt = newOpportunity.CreatedAt
            };
        }

        public async Task<OpportunityResponse> UpdateOpportunityAsync(Guid opportunityId, UpsertOpportunityRequest upsertOpportunity, CancellationToken cancellationToken)
        {
            Opportunity opportunity = await UpdateOpportunityEntityAsync(opportunityId, upsertOpportunity, cancellationToken);

            return new OpportunityResponse
            {
                LeadId = opportunity.LeadId,
                Value = opportunity.Value,
                Stage = opportunity.Stage,
                CreatedAt = opportunity.CreatedAt
            };
        }

        public async Task<Unit> DeleteOpportunityAsync(Guid opportunityId, CancellationToken cancellationToken)
        {
            await DeleteOpportunityEntityAsync(opportunityId, cancellationToken);

            return default;
        }

        #endregion Public methods

        #region Private methods

        private async Task<Opportunity> InsertOpportunityEntityAsync(UpsertOpportunityRequest upsertOpportunity, CancellationToken cancellationToken)
        {
            var newOpportunity = new Opportunity
            {
                Id = Guid.NewGuid(),
                LeadId = upsertOpportunity.LeadId,
                Value = upsertOpportunity.Value,
                Stage = upsertOpportunity.Stage,
                CreatedAt = DateTime.UtcNow
            };

            await context.Opportunities.AddAsync(newOpportunity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return newOpportunity;
        }

        private async Task<Opportunity> UpdateOpportunityEntityAsync(Guid opportunityId, UpsertOpportunityRequest upsertOpportunity, CancellationToken cancellationToken)
        {
            Opportunity opportunity = await context.Opportunities.FirstOrDefaultAsync(x => x.Id == opportunityId, cancellationToken);

            opportunity.LeadId = upsertOpportunity.LeadId;
            opportunity.Value = upsertOpportunity.Value;
            opportunity.Stage = upsertOpportunity.Stage;
            await context.SaveChangesAsync(cancellationToken);

            return opportunity;
        }

        private async Task<Unit> DeleteOpportunityEntityAsync(Guid opportunityId, CancellationToken cancellationToken)
        {
            var opportunity = await context.Opportunities.FirstOrDefaultAsync(x => x.Id == opportunityId, cancellationToken);

            if (opportunity == null)
                return default;

            context.Opportunities.Remove(opportunity);
            await context.SaveChangesAsync(cancellationToken);
            return default;
        }

        #endregion Private methods

    }

    #region Commands

    public record InsertOpportunityCommand(UpsertOpportunityRequest UpsertOpportunityRequest) : IRequest<OpportunityResponse>;
    public record UpdateOpportunityCommand(Guid OpportunityId, UpsertOpportunityRequest UpsertOpportunityRequest) : IRequest<OpportunityResponse>;
    public record DeleteOpportunityCommand(Guid OpportunityId) : IRequest<Unit>;

    #endregion Commands
}