using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimizer.Common.Constants;
using Minimizer.Common.Exceptions;
using Minimizer.Common.Models.Leads;
using Minimizer.Entities.Models;
using Minimizer.Services.Leads;
using System.Net;

namespace Minimizer.API.Controllers
{
    [ApiController]
    [Route("api/leads")]
    [Authorize]
    public class LeadController : ControllerBase
    {
        private readonly IMediator mediator;

        public LeadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<LeadResponse>> GetAllLeads()
        {
            return await mediator.Send(new GetAllLeadsQuery());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string leadId)
        {
            if (!Guid.TryParse(leadId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(leadId), string.Format(ErrorContants.InvalidGuid, nameof(leadId), leadId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            var lead = await mediator.Send(new GetLeadByIdQuery(guidId));
            if (lead == null) return NotFound();
            return Ok(lead);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] UpsertLeadRequest upsertLeadRequest)
        {
            LeadResponse newLead = await mediator.Send(new InsertLeadCommand(upsertLeadRequest));
            return Ok(newLead);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLead(string leadId, [FromBody] UpsertLeadRequest upsertLeadRequest)
        {
            if (!Guid.TryParse(leadId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(leadId), string.Format(ErrorContants.InvalidGuid, nameof(leadId), leadId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            bool leadExists = await mediator.Send(new CheckLeadByIdQuery(guidId));
            
            if (!leadExists) 
                return NotFound();

            LeadResponse updatedLead = await mediator.Send(new UpdateLeadCommand(guidId, upsertLeadRequest));
            return Ok(updatedLead);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string leadId)
        {
            if (!Guid.TryParse(leadId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(leadId), string.Format(ErrorContants.InvalidGuid, nameof(leadId), leadId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            await mediator.Send(new DeleteLeadCommand(guidId));
            return NoContent();
        }
    }
}