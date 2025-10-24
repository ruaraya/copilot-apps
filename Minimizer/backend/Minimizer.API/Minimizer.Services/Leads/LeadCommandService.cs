using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Leads;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Leads
{
    public class LeadCommandService :
        IRequestHandler<InsertLeadCommand, LeadResponse>,
        IRequestHandler<UpdateLeadCommand, LeadResponse>,
        IRequestHandler<DeleteLeadCommand, Unit>
    {
        private readonly MinimizerDbContext context;

        public LeadCommandService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<LeadResponse> Handle(InsertLeadCommand request,
            CancellationToken cancellationToken) =>

            await InsertLeadAsync(request.UpsertLeadRequest, cancellationToken);        

        public async Task<LeadResponse> Handle(UpdateLeadCommand request,
            CancellationToken cancellationToken) =>

            await UpdateLeadAsync(request.LeadId, request.UpsertLeadRequest, cancellationToken);

        public async Task<Unit> Handle(DeleteLeadCommand request,
            CancellationToken cancellationToken) =>

            await DeleteLeadAsync(request.LeadId, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<LeadResponse> InsertLeadAsync(UpsertLeadRequest upsertLead, CancellationToken cancellationToken)
        {
            Lead newLead = await InsertLeadEntityAsync(upsertLead, cancellationToken);

            return new LeadResponse
            {
                Name = newLead.Name,
                Email = newLead.Email,
                Status = newLead.Status,
                CreatedAt = newLead.CreatedAt
            };
        }

        public async Task<LeadResponse> UpdateLeadAsync(Guid leadId, UpsertLeadRequest upsertLead, CancellationToken cancellationToken)
        {
            Lead updatedLead = await UpdateLeadEntityAsync(leadId, upsertLead, cancellationToken);

            return new LeadResponse
            {
                Name = updatedLead.Name,
                Email = updatedLead.Email,
                Status = updatedLead.Status,
                CreatedAt = updatedLead.CreatedAt
            };
        }

        public async Task<Unit> DeleteLeadAsync(Guid leadId, CancellationToken cancellationToken)
        {
            await DeleteLeadEntityAsync(leadId, cancellationToken);

            return default;
        }

        #endregion Public methods

        #region Private methods

        private async Task<Lead> InsertLeadEntityAsync(UpsertLeadRequest upsertLead, CancellationToken cancellationToken)
        {
            var newLead = new Lead
            {
                Id = Guid.NewGuid(),
                Name = upsertLead.Name,
                Email = upsertLead.Email,
                Status = upsertLead.Status,
                CreatedAt = DateTime.UtcNow
            };

            await context.Leads.AddAsync(newLead, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return newLead;
        }

        private async Task<Lead> UpdateLeadEntityAsync(Guid leadId, UpsertLeadRequest upsertLead, CancellationToken cancellationToken)
        {
            var lead = await context.Leads.FirstOrDefaultAsync(x => x.Id == leadId, cancellationToken);
            
            if (lead == null) 
                return null;

            lead.Name = upsertLead.Name;
            lead.Email = upsertLead.Email;
            lead.Status = upsertLead.Status;
            await context.SaveChangesAsync(cancellationToken);

            return lead;
        }

        private async Task<Unit> DeleteLeadEntityAsync(Guid leadId, CancellationToken cancellationToken)
        {
            var lead = await context.Leads.FirstOrDefaultAsync(x => x.Id == leadId, cancellationToken);
            
            if (lead == null) 
                return default;

            context.Leads.Remove(lead);
            await context.SaveChangesAsync(cancellationToken);
            return default;
        }

        #endregion Private methods
    }

    #region Commands

    public record InsertLeadCommand(UpsertLeadRequest UpsertLeadRequest) : IRequest<LeadResponse>;
    public record UpdateLeadCommand(Guid LeadId, UpsertLeadRequest UpsertLeadRequest) : IRequest<LeadResponse>;
    public record DeleteLeadCommand(Guid LeadId) : IRequest<Unit>;

    #endregion Commands
}