using MyLab.EmailManager.Domain.Dto;
using MyLab.EmailManager.Domain.Exceptions;

namespace MyLab.EmailManager.Domain.Entities;

public class Sending
{
    public SendMessageDef Message { get; set; }

    public static Sending Create(SendMessageDef message)
    {
        if (message == null) 
            throw new ArgumentNullException(nameof(message));

        if (message.Selection.Length == 0)
            throw new DomainException("Selection labels are required");

        if (message.GenericMsg == null && message.SimpleMsg == null)
            throw new DomainException("Any message content is required");

        if (message.GenericMsg != null && message.SimpleMsg != null)
            throw new DomainException("Only one message content case is supported");

        return new Sending(message);
    }

    Sending(SendMessageDef message)
    {
        Message = message;
    }
}