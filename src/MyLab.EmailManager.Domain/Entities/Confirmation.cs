using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities;

public class Confirmation(Guid emailId)
{
    public Guid EmailId { get; } = emailId;

    public DatedValue<ConfirmationStep> Step{ get; private set; } = DatedValue<ConfirmationStep>.Unset;

    public Confirmation(Guid emailId, ConfirmationStep initialStep)
        : this(emailId)
    {
        Step = DatedValue<ConfirmationStep>.CreateSet(initialStep);
    }

    public void ChangeStep(ConfirmationStep newStep)
    {
        if (newStep <= Step.Value)
            throw new InvalidNewConfirmationStepException(Step.Value, newStep);

        Step = DatedValue<ConfirmationStep>.CreateSet(newStep);
    }
}