using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.EmailManager.App.Exceptions;
using MyLab.EmailManager.App.Features.GetConfirmation;
using MyLab.EmailManager.App.Features.RepeatConfirmation;
using MyLab.WebErrors;

namespace MyLab.EmailManager.Confirmations;

[ApiController]
[Route("emails/{email_id}/confirmation")]
public class EmailConfirmationController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("new")]
    [ErrorToResponse(typeof(NotFoundException), HttpStatusCode.NotFound)]
    public async Task<IActionResult> Repeat([FromRoute(Name = "email_id")] Guid emailId)
    {
        await mediator.Send(new RepeatConfirmationCommand(emailId));
        return Ok();
    }

    [HttpGet("state")]
    [ErrorToResponse(typeof(NotFoundException), HttpStatusCode.NotFound)]
    public async Task<IActionResult> Get([FromRoute(Name = "email_id")] Guid emailId)
    {
        var vm = await mediator.Send(new GetConfirmationCommand(emailId));
        return Ok(mapper.Map<ConfirmationStateDto>(vm));
    }
}