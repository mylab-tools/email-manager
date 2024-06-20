using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Infrastructure.UnitTests;

public partial class AppDbContextBehavior : IAsyncLifetime
{
    [Fact]
    public void ShouldNotStoreConfirmationWithoutEmail()
    {
        //Arrange
        var confirmation = new Confirmation(Guid.NewGuid());

        //Act
        _dbContext.Confirmations.Add(confirmation);

        //Assert
        Assert.Throws<DbUpdateException>(() => _dbContext.SaveChanges());
    }

    [Fact]
    public void ShouldStoreConfirmation()
    {
        //Arrange
        var email = SaveTestEmail();
        var confirmation = new Confirmation(email.Id, ConfirmationStep.Created);

        //Act
        _dbContext.Confirmations.Add(confirmation);
        _dbContext.SaveChanges();

        var foundConfirmation = _dbContext.Confirmations.FirstOrDefault(c => c.EmailId == email.Id);

        //Assert
        Assert.NotNull(foundConfirmation);
        Assert.Equal(ConfirmationStep.Created, foundConfirmation.Step.Value);
        Assert.NotNull(foundConfirmation.Step.DateTime);
    }

    [Fact]
    public void ShouldChangeStep()
    {
        //Arrange
        var email = SaveTestEmail();
        var confirmation = new Confirmation(email.Id, ConfirmationStep.Created);
        _dbContext.Confirmations.Add(confirmation);
        _dbContext.SaveChanges();

        //Act
        confirmation.ChangeStep(ConfirmationStep.Confirmed);

        var storedConfirmation = _dbContext.Confirmations.FirstOrDefault(c => c.EmailId == email.Id);

        //Assert
        Assert.NotNull(storedConfirmation);
        Assert.Equal(ConfirmationStep.Confirmed, storedConfirmation.Step.Value);
        Assert.NotNull(storedConfirmation.Step.DateTime);
    }

    [Fact] public void ShouldBeDeletedWithEmail()
    {
        //Arrange
        var email = SaveTestEmail();
        var confirmation = new Confirmation(email.Id, ConfirmationStep.Created);
        _dbContext.Confirmations.Add(confirmation);
        _dbContext.SaveChanges();

        //Act
        _dbContext.Emails.Remove(email);
        _dbContext.SaveChanges();

        bool confirmationExists = _dbContext.Confirmations.Any(c => c.EmailId == email.Id);

        //Assert
        Assert.False(confirmationExists);
    }
}