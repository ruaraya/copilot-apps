using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Leads;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Leads
{
    public class LeadQueryService :
        IRequestHandler<CheckLeadByIdQuery, bool>,
        IRequestHandler<GetAllLeadsQuery, List<LeadResponse>>,
        IRequestHandler<GetLeadByIdQuery, LeadResponse?>
    {
        private readonly MinimizerDbContext context;

        public LeadQueryService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<bool> Handle(CheckLeadByIdQuery request, CancellationToken cancellationToken) =>
            await context.Leads.AnyAsync(x => x.Id == request.LeadId, cancellationToken);

        public async Task<List<LeadResponse>> Handle(GetAllLeadsQuery request,
            CancellationToken cancellationToken) =>

            await GetAllLeadsAsync(cancellationToken);


        public async Task<LeadResponse?> Handle(GetLeadByIdQuery request,
            CancellationToken cancellationToken) =>

            await GetLeadByLeadIdAsync(request.LeadId, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<List<LeadResponse>> GetAllLeadsAsync(CancellationToken cancellationToken)
        {
            var leads = await GetAllLeadEntitiesAsync(cancellationToken);
            
            if (!leads.Any()) 
                return new List<LeadResponse>();

            return leads.Select(l => new LeadResponse
            {
                Name = l.Name,
                Email = l.Email,
                Status = l.Status,
                CreatedAt = l.CreatedAt
            }).ToList();
        }

        public async Task<LeadResponse?> GetLeadByLeadIdAsync(Guid leadId, CancellationToken cancellationToken)
        {
            var lead = await context.Leads.FirstOrDefaultAsync(x => x.Id == leadId, cancellationToken);
            if (lead == null) 
                return null;

            return new LeadResponse
            {
                Name = lead.Name,
                Email = lead.Email,
                Status = lead.Status,
                CreatedAt = lead.CreatedAt
            };
        }

        #endregion Public methods

        #region Private methods

        private async Task<List<Lead>> GetAllLeadEntitiesAsync(CancellationToken cancellationToken)
        {
            return await context.Leads.ToListAsync(cancellationToken);
        }

        private async Task<Lead?> GetLeadEntityByLeadIdAsync(Guid leadId, CancellationToken cancellationToken)
        {
            return await context.Leads.FirstOrDefaultAsync(x => x.Id == leadId, cancellationToken);
        }

        #endregion Private methods
    }

    #region Queries

    public record CheckLeadByIdQuery(Guid LeadId) : IRequest<bool>;
    public record GetAllLeadsQuery() : IRequest<List<LeadResponse>>;
    public record GetLeadByIdQuery(Guid LeadId) : IRequest<LeadResponse?>;

    #endregion Queries
}