using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.EfTypeConfigurations;

namespace Infrastructure.UnitTests;

public partial class AppDbContextBehavior
{
    [Fact]
    public void ShouldStoreEmail()
    {
        //Arrange
        var emailId = Guid.NewGuid();
        var newEmail = new Email(emailId, "foo@host.com");

        //Act
        _dbContext.Emails.Add(newEmail);
        _dbContext.SaveChanges();

        var storedEmails = _dbContext.Emails
            .Where(e => e.Id == emailId)
            .ToArray();

        var storedEmail = storedEmails?.FirstOrDefault();

        //Assert
        Assert.NotNull(storedEmails);
        Assert.Single(storedEmails);
        Assert.NotNull(storedEmail);
        Assert.Equal(emailId, storedEmail.Id);
        Assert.Equal("foo@host.com", storedEmail.Address.Value);
        Assert.False(storedEmail.Deletion.Value);
        Assert.Null(storedEmail.Deletion.DateTime);
    }

    [Fact]
    public void ShouldAddEmailLabels()
    {
        //Arrange
        var newEmail = SaveTestEmail();

        //Act
        newEmail.Labels.Add(new EmailLabel("foo", "bar"));
        _dbContext.SaveChanges();

        var allEmailLabels = _dbContext.EmailLabels.ToArray();
        var emailLabel = allEmailLabels.FirstOrDefault();

        string? labelToEmailId = null;

        if (emailLabel != null)
        {
            labelToEmailId = _dbContext.ChangeTracker
                .Entries<EmailLabel>()
                .Single(e => e.Entity == emailLabel)
                .Properties
                .Single(p => p.Metadata.Name == EmailEntityTypeConfiguration.EmailLabelToEmailFkFieldName)
                .CurrentValue
                ?.ToString();
        }

        //Assert
        Assert.NotNull(allEmailLabels);
        Assert.Single(allEmailLabels);
        Assert.NotNull(emailLabel);
        Assert.Equal("foo", emailLabel.Name.Text);
        Assert.Equal("bar", emailLabel.Value);
        Assert.NotNull(labelToEmailId);
        Assert.Equal(newEmail.Id, Guid.Parse(labelToEmailId));
    }

    [Fact]
    public void ShouldSaveEmailDeletion()
    {
        //Arrange
        var newEmail = SaveTestEmail();

        //Act
        newEmail.Delete();
        _dbContext.SaveChanges();

        var storedEmail = _dbContext.Emails.FirstOrDefault(e => e.Id == newEmail.Id);

        //Assert
        Assert.NotNull(storedEmail);
        Assert.NotNull(storedEmail.Deletion);
        Assert.NotNull(storedEmail.Deletion.DateTime);
        Assert.True(storedEmail.Deletion.Value);
    }

    [Fact]
    public void ShouldCascadeDeleteLabels()
    {
        //Arrange
        var newEmail = SaveTestEmail();
        newEmail.Labels.Add(new EmailLabel("foo", "bar"));
        _dbContext.SaveChanges();

        //Act

        _dbContext.Emails.Remove(newEmail);
        _dbContext.SaveChanges();

        var anyLabelsExists = _dbContext.EmailLabels.Any();
        
        //Assert
        Assert.False(anyLabelsExists);
    }
}