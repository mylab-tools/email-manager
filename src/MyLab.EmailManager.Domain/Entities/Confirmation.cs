using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities;

public class Confirmation(Guid emailId)
{
    public Guid Seed { get; private init; } = Guid.Empty;

    public Guid EmailId { get; } = emailId;

    public DatedValue<ConfirmationStep> Step{ get; private set; } = DatedValue<ConfirmationStep>.Unset;

    public static Confirmation CreateNew(Guid emailId)
    {
        return new Confirmation(emailId, ConfirmationStep.Created)
        {
            Seed = Guid.NewGuid()
        };
    }

    internal Confirmation(Guid emailId, ConfirmationStep initialStep)
        : this(emailId)
    {
        Step = DatedValue<ConfirmationStep>.CreateSet(initialStep);
    }

    public void Reset()
    {
        Step = DatedValue<ConfirmationStep>.CreateSet(ConfirmationStep.Created);
    }

    public void ToSentState()
    {
        Step = DatedValue<ConfirmationStep>.CreateSet(ConfirmationStep.Sent);
    }

    public void Complete(Guid seed)
    {
        if (seed != Seed)
            throw new InvalidConfirmationSeedException();
        Step = DatedValue<ConfirmationStep>.CreateSet(ConfirmationStep.Confirmed);
    }
}