using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.EmailManager.App.Features.CreateEmail;
using MyLab.EmailManager.App.Features.CreateOrUpdateEmail;
using MyLab.EmailManager.App.Features.GetEmail;
using MyLab.EmailManager.App.Features.SoftDeleteEmail;

namespace MyLab.EmailManager.Emails
{
    [ApiController]
    [Route("emails")]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IValidator<EmailDefDto> _emailDefValidator;

        public EmailController
        (
            IMediator mediator,
            IMapper mapper,
            IValidator<EmailDefDto> emailDefValidator
        )
        {
            _mediator = mediator;
            _mapper = mapper;
            _emailDefValidator = emailDefValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "email_id")]Guid emailId)
        {
            var emailVm = await _mediator.Send(new GetEmailQuery(emailId));
            var dto = _mapper.Map<EmailViewModelDto>(emailVm);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmailDefDto emailDef)
        {
            var valRes = await _emailDefValidator.ValidateAsync(emailDef);
            if (!valRes.IsValid)
                return BadRequest(valRes.ToString());

            var cmd = _mapper.Map<CreateEmailCommand>(emailDef);
            var resp = await _mediator.Send(cmd);
            return Ok(resp.Id);
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrUpdate([FromQuery(Name = "email_id")] Guid emailId, [FromBody] EmailDefDto emailDef)
        {
            var valRes = await _emailDefValidator.ValidateAsync(emailDef);
            if (!valRes.IsValid)
                return BadRequest(valRes.ToString());

            var cmd = new CreateOrUpdateEmailCommand(emailId, emailDef.Address!, emailDef.Labels);
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery(Name = "email_id")] Guid emailId)
        {
            await _mediator.Send(new SoftDeleteEmailCommand(emailId));
            return Ok();
        }
    }
}
