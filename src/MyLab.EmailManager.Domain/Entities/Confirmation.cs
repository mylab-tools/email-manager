using System.Diagnostics.Contracts;
using System.Xml.Serialization;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Entities;

public class Confirmation(Guid emailId)
{
    public Guid EmailId { get; } = emailId;

    public DatedValue<bool> Deletion { get; private set; } = DatedValue<bool>.Unset;

    public DatedValue<ConfirmationStep> Step{ get; private set; } = DatedValue<ConfirmationStep>.Unset;

    public Confirmation(Guid emailId, ConfirmationStep initialStep)
        : this(emailId)
    {
        Step = DatedValue<ConfirmationStep>.CreateSet(initialStep);
    }

    public void Delete()
    {
        if (Deletion.Value)
            throw new DomainException("An entity already has been deleted");

        Deletion = DatedValue<bool>.CreateSet(true);
    }

    public void ChangeStep(ConfirmationStep newStep)
    {
        if (Deletion.Value)
            throw new DomainException("An entity has been deleted");

        if (newStep <= Step.Value)
            throw new InvalidNewConfirmationStepException(Step.Value, newStep);

        Step = DatedValue<ConfirmationStep>.CreateSet(newStep);
    }
}