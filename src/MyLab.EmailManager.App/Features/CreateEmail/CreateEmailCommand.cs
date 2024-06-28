using MediatR;

namespace MyLab.EmailManager.App.Features.CreateEmail;

public class CreateEmailCommand(string address, IDictionary<string, string>? labels) : IRequest<CreateEmailResponse>
{
    public string Address { get; } = address;
    public IReadOnlyDictionary<string, string>? Labels { get; } = labels?.AsReadOnly();
}