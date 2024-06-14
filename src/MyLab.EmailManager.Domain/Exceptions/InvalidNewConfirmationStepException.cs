using MyLab.EmailManager.Domain.ValueObjects;

namespace MyLab.EmailManager.Domain.Exceptions;

public class InvalidNewConfirmationStepException(ConfirmationStep curStep, ConfirmationStep newStep)
    : DomainException("Invalid new confirmation step")
{
    public ConfirmationStep NewStep { get; set; } = newStep;
    public ConfirmationStep CurrentStep { get; set; } = curStep;
}