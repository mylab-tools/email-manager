using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.EmailManager.App.Exceptions;
using MyLab.EmailManager.App.Features.CompleteConfirmation;
using MyLab.WebErrors;

namespace MyLab.EmailManager.Confirmations
{
    [ApiController]
    [Route("confirmations/completed")]
    public class ConfirmationController(IMediator mediator) : ControllerBase
    {
        [HttpPost("{seed}")]
        [ErrorToResponse(typeof(NotFoundException), HttpStatusCode.NotFound)]
        public async Task<IActionResult> Complete([FromRoute] Guid seed)
        {
            await mediator.Send(new CompleteConfirmationCommand(seed));
            return Ok();
        }
    }
}
