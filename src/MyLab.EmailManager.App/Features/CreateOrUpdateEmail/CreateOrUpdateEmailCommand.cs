using MediatR;
using MyLab.EmailManager.App.ViewModels;

namespace MyLab.EmailManager.App.Features.CreateOrUpdateEmail;

public class CreateOrUpdateEmailCommand(Guid emailId, string address, IDictionary<string, string>? labels) : IRequest
{
    public Guid EmailId { get; } = emailId;
    public string Address { get; } = address;
    public IReadOnlyDictionary<string, string>? Labels { get; } = labels?.AsReadOnly();
}