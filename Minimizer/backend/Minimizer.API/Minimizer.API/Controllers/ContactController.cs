using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimizer.Common.Constants;
using Minimizer.Common.Exceptions;
using Minimizer.Common.Models.Contacts;
using Minimizer.Services.Contacts;
using System.Net;

namespace Minimizer.API.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IMediator mediator;

        public ContactController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<ContactResponse>> GetAllContacts()
        {
            return await mediator.Send(new GetAllContactsQuery());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(string contactId)
        {
            if (!Guid.TryParse(contactId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(contactId), string.Format(ErrorContants.InvalidGuid, nameof(contactId), contactId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            var contact = await mediator.Send(new GetContactByContactIdQuery(guidId));
            
            if (contact == null) 
                return NotFound();
            
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] UpsertContactRequest upsertContactRequest)
        {
            ContactResponse newContact = await mediator.Send(new InsertContactCommand(upsertContactRequest));
            return Ok(newContact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(string contactId, [FromBody] UpsertContactRequest upsertContactRequest)
        {
            if (!Guid.TryParse(contactId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(contactId), string.Format(ErrorContants.InvalidGuid, nameof(contactId), contactId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            bool contactExists = await mediator.Send(new CheckContactByContactIdQuery(guidId));
            
            if (!contactExists) 
                return NotFound();

            ContactResponse updatedContact = await mediator.Send(new UpdateContactCommand(guidId, upsertContactRequest));
            return Ok(updatedContact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string contactId)
        {
            if (!Guid.TryParse(contactId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(contactId), string.Format(ErrorContants.InvalidGuid, nameof(contactId), contactId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            await mediator.Send(new DeleteContactCommand(guidId));
            return NoContent();
        }
    }
}