using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Contacts;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Contacts
{
    public class ContactQueryService :
        IRequestHandler<CheckContactByContactIdQuery, bool>,
        IRequestHandler<GetAllContactsQuery, List<ContactResponse>>,
        IRequestHandler<GetContactByContactIdQuery, ContactResponse?>
    {
        private readonly MinimizerDbContext context;

        public ContactQueryService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<bool> Handle(CheckContactByContactIdQuery request, CancellationToken cancellationToken) =>
            await context.Contacts.AnyAsync(x => x.Id == request.ContactId, cancellationToken);

        public async Task<List<ContactResponse>> Handle(GetAllContactsQuery request, 
            CancellationToken cancellationToken) =>

            await GetAllUsersAsync(cancellationToken);

        public async Task<ContactResponse?> Handle(GetContactByContactIdQuery request,
            CancellationToken cancellationToken) =>

            await GetContactByContactIdAsync(request.ContactId, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<List<ContactResponse>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var contactEntities = await GetAllContactEntitiesAsync(cancellationToken);
            if (!contactEntities.Any()) return new List<ContactResponse>();

            var contacts = contactEntities.Select(c => new ContactResponse
            {
                LeadId = c.LeadId,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Position = c.Position
            }).ToList();

            return contacts;
        }

        public async Task<ContactResponse?> GetContactByContactIdAsync(Guid contactId, CancellationToken cancellationToken)
        {
            var contact = await GetContactEntityByContactIdAsync(contactId, cancellationToken);
            if (contact == null) return null;

            return new ContactResponse
            {
                LeadId = contact.LeadId,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Position = contact.Position
            };
        }

        #endregion Public methods

        #region Private methods

        private async Task<List<Contact>> GetAllContactEntitiesAsync(CancellationToken cancellationToken)
        {
            return await context.Contacts.ToListAsync(cancellationToken);
        }

        private async Task<Contact?> GetContactEntityByContactIdAsync(Guid contactId, CancellationToken cancellationToken)
        {
            return await context.Contacts.FirstOrDefaultAsync(x => x.Id == contactId, cancellationToken);
        }

        #endregion Private methods
    }

    #region Queries

    public record CheckContactByContactIdQuery(Guid ContactId) : IRequest<bool>;
    public record GetAllContactsQuery() : IRequest<List<ContactResponse>>;
    public record GetContactByContactIdQuery(Guid ContactId) : IRequest<ContactResponse?>;

    #endregion Queries
}