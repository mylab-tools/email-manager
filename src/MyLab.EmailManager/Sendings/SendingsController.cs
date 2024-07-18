using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.EmailManager.App.Features.CreateSending;
using MyLab.EmailManager.App.Features.GetSending;

namespace MyLab.EmailManager.Sendings
{
    [ApiController]
    [Route("sendings")]
    public class SendingsController(IMediator mediator, IMapper mapper) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SendingDefDto sendingDef)
        {
            var sendingDefValidator = new SendingDefDtoValidator();
            var validationRes = await sendingDefValidator.ValidateAsync(sendingDef);

            if (!validationRes.IsValid)
                return BadRequest(validationRes.Errors);

            var command = mapper.Map<CreateSendingCommand>(sendingDef);

            var resp = await mediator.Send(command);

            return Ok(resp.SendingId);
        }

        [HttpGet("{sending_id}")]
        public async Task<IActionResult> Get([FromRoute(Name = "sending_id")] string sendingId)
        {
            if (string.IsNullOrWhiteSpace(sendingId))
                return BadRequest("sending_id is required");
            if (!Guid.TryParse(sendingId, out var sendingIdGuid))
                return BadRequest("sending_id must have GUID format");

            var vm = await mediator.Send(new GetSendingQuery(sendingIdGuid));
            SendingViewModelDto? vmDto;
            try
            {
                vmDto = mapper.Map<SendingViewModelDto>(vm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return vm != null ? Ok(vmDto) : NotFound("Sending not found");
        }
    }
}
