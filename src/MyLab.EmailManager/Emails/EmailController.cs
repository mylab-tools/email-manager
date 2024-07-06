using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.EmailManager.App.Features.CreateEmail;
using MyLab.EmailManager.App.Features.CreateOrUpdateEmail;
using MyLab.EmailManager.App.Features.GetEmail;
using MyLab.EmailManager.App.Features.SoftDeleteEmail;
using MyLab.EmailManager.Common;

namespace MyLab.EmailManager.Emails
{
    [ApiController]
    [Route("emails")]
    public class EmailController(
        IMediator mediator,
        IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "email_id")]Guid emailId)
        {
            var emailVm = await mediator.Send(new GetEmailQuery(emailId));
            var dto = mapper.Map<EmailViewModelDto>(emailVm);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmailDefDto emailDef)
        {
            var valRes = await EmailDefValidator.Instance.ValidateAsync(emailDef);
            if (!valRes.IsValid)
                return BadRequest(valRes.ToString());

            var cmd = mapper.Map<CreateEmailCommand>(emailDef);
            var resp = await mediator.Send(cmd);
            return Ok(resp.Id);
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrUpdate([FromQuery(Name = "email_id")] Guid emailId, [FromBody] EmailDefDto emailDef)
        {
            var valRes = await EmailDefValidator.Instance.ValidateAsync(emailDef);
            if (!valRes.IsValid)
                return BadRequest(valRes.ToString());

            var cmd = new CreateOrUpdateEmailCommand(emailId, emailDef.Address!, emailDef.Labels);
            await mediator.Send(cmd);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery(Name = "email_id")] Guid emailId)
        {
            await mediator.Send(new SoftDeleteEmailCommand(emailId));
            return Ok();
        }
    }
}
