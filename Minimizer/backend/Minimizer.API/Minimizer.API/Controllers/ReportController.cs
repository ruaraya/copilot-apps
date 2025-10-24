using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minimizer.Common.Models.Reports;
using Minimizer.Services.Reports;

namespace Minimizer.API.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("lead-conversion")]
        public async Task<List<ReportResponse>> GetLeadConversionReport()
        {
            return await mediator.Send(new GetLeadConversionReportQuery());
        }
    }
}