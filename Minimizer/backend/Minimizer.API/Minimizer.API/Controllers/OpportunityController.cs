using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimizer.Common.Constants;
using Minimizer.Common.Exceptions;
using Minimizer.Common.Models.Opportunities;
using Minimizer.Services.Opportunities;
using System.Net;

namespace Minimizer.API.Controllers
{
    [ApiController]
    [Route("api/opportunities")]
    [Authorize]
    public class OpportunityController : ControllerBase
    {
        private readonly IMediator mediator;

        public OpportunityController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<OpportunityResponse>> GetAllOpportunities()
        {
            return await mediator.Send(new GetAllOpportunitiesQuery());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOpportunityById(string opportunityId)
        {
            if (!Guid.TryParse(opportunityId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(opportunityId), string.Format(ErrorContants.InvalidGuid, nameof(opportunityId), opportunityId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            var opportunity = await mediator.Send(new GetOpportunityByOpportunityIdQuery(guidId));
            if (opportunity == null) return NotFound();
            return Ok(opportunity);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOpportunity([FromBody] UpsertOpportunityRequest upsertOpportunityRequest)
        {
            OpportunityResponse newOpportunity = await mediator.Send(new InsertOpportunityCommand(upsertOpportunityRequest));
            return Ok(newOpportunity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpportunity(string opportunityId, [FromBody] UpsertOpportunityRequest upsertOpportunityRequest)
        {
            if (!Guid.TryParse(opportunityId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(opportunityId), string.Format(ErrorContants.InvalidGuid, nameof(opportunityId), opportunityId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            bool opportunityExists = await mediator.Send(new CheckOpportunityByOpportunityIdQuery(guidId));
            if (!opportunityExists) return NotFound();

            OpportunityResponse updatedOpportunity = await mediator.Send(new UpdateOpportunityCommand(guidId, upsertOpportunityRequest));
            return Ok(updatedOpportunity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string opportunityId)
        {
            if (!Guid.TryParse(opportunityId, out Guid guidId))
            {
                ModelState.AddModelError(nameof(opportunityId), string.Format(ErrorContants.InvalidGuid, nameof(opportunityId), opportunityId));

                throw new ApiException(ModelState, (int)HttpStatusCode.BadRequest);
            }

            await mediator.Send(new DeleteOpportunityCommand(guidId));
            return NoContent();
        }
    }
}