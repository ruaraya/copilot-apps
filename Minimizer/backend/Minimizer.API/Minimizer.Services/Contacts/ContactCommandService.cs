using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Contacts;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Contacts
{   
    public class ContactCommandService :
        IRequestHandler<InsertContactCommand, ContactResponse>,
        IRequestHandler<UpdateContactCommand, ContactResponse>,
        IRequestHandler<DeleteContactCommand, Unit>
    {
        private readonly MinimizerDbContext context;

        public ContactCommandService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<ContactResponse> Handle(InsertContactCommand request,
            CancellationToken cancellationToken) =>

            await InsertContactAsync(request.UpsertContactRequest, cancellationToken);

        public async Task<ContactResponse> Handle(UpdateContactCommand request,
            CancellationToken cancellationToken) =>

            await UpdateContactAsync(request.ContactId, request.UpsertContactRequest, cancellationToken);

        public async Task<Unit> Handle(DeleteContactCommand request,
            CancellationToken cancellationToken) =>

            await DeleteContactAsync(request.ContactId, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<ContactResponse> InsertContactAsync(UpsertContactRequest request, CancellationToken cancellationToken)
        {
            Contact newContact = await InsertContactEntityAsync(request, cancellationToken);

            return new ContactResponse
            {
                LeadId = newContact.LeadId,
                Name = newContact.Name,
                Phone = newContact.Phone,
                Email = newContact.Email,
                Position = newContact.Position
            };
        }

        public async Task<ContactResponse> UpdateContactAsync(Guid contactId, UpsertContactRequest upsertContact, CancellationToken cancellationToken)
        {
            Contact contact = await UpdateContactEntityAsync(contactId, upsertContact, cancellationToken);

            return new ContactResponse
            {
                LeadId = contact.LeadId,
                Name = contact.Name,
                Phone = contact.Phone,
                Email = contact.Email,
                Position = contact.Position
            };
        }

        public async Task<Unit> DeleteContactAsync(Guid contactId, CancellationToken cancellationToken)
        {
            await DeleteContactEntityAsync(contactId, cancellationToken);

            return default;
        }

        #endregion Public methods

        #region Private methods

        private async Task<Contact> InsertContactEntityAsync(UpsertContactRequest upsertContact, CancellationToken cancellationToken)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                LeadId = upsertContact.LeadId,
                Name = upsertContact.Name,
                Phone = upsertContact.Phone,
                Email = upsertContact.Email,
                Position = upsertContact.Position
            };

            await context.Contacts.AddAsync(contact, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return contact;
        }

        private async Task<Contact> UpdateContactEntityAsync(Guid contactId, UpsertContactRequest upsertContact, CancellationToken cancellationToken)
        {
            var contact = await context.Contacts.FirstOrDefaultAsync(x => x.Id == contactId, cancellationToken);
            if (contact == null) return null;

            contact.LeadId = upsertContact.LeadId;
            contact.Name = upsertContact.Name;
            contact.Phone = upsertContact.Phone;
            contact.Email = upsertContact.Email;
            contact.Position = upsertContact.Position;
            await context.SaveChangesAsync(cancellationToken);

            return contact;
        }

        private async Task<Unit> DeleteContactEntityAsync(Guid contactId, CancellationToken cancellationToken)
        {
            var contact = await context.Contacts.FirstOrDefaultAsync(x => x.Id == contactId, cancellationToken);
            if (contact == null) return default;

            context.Contacts.Remove(contact);
            await context.SaveChangesAsync(cancellationToken);
            return default;
        }

        #endregion Private methods
    }

    #region Commands

    public record InsertContactCommand(UpsertContactRequest UpsertContactRequest) : IRequest<ContactResponse>;
    public record UpdateContactCommand(Guid ContactId, UpsertContactRequest UpsertContactRequest) : IRequest<ContactResponse>;
    public record DeleteContactCommand(Guid ContactId) : IRequest<Unit>;

    #endregion Commands
}