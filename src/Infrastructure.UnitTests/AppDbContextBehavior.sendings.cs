using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Infrastructure.UnitTests;

public partial class AppDbContextBehavior
{
    [Fact]
    public async Task  ShouldStoreSimpleSending()
    {
        //Arrange
        var sendingId = Guid.NewGuid();

        var email = await SaveTestEmailAsync();

        var initialSelection = new EmailLabel[]
        {
            new("foo", "bar")
        };

        var sendingMessage = EmailMessage.New
        (
            email.Id,
            email.Address,
            "baz",
            new TextContent("qoz", false)
        );

        var newSending = new Sending
        (
            sendingId,
            initialSelection,
            new SimpleMessageContent("qoz")
        )
        {
            Messages = new List<EmailMessage>
            {
                sendingMessage
            }
        };
        //Act
        
        _dbContext.Sendings.Add(newSending);
        await _dbContext.SaveChangesAsync();

        var storedSending = _dbContext.Sendings
            .Include(sending => sending.Messages)
            .FirstOrDefault(s => s.Id == newSending.Id);

        //Assert
        Assert.NotNull(storedSending);
        Assert.Equal(sendingId, storedSending.Id);
        Assert.Single(storedSending.Selection);
        Assert.True(storedSending.Selection.First() is
        {
            Name.Text: "foo", 
            Value.Text:"bar"
        });
        Assert.NotNull(storedSending.Messages);
        Assert.Single(storedSending.Messages);

        var storedMessage = storedSending.Messages.First();
        Assert.True(storedMessage is
        {
            Title.Text: "baz",
            Content.Text: "qoz",
            Content.IsHtml: false
        });
        Assert.Equal(sendingMessage.Id, storedMessage.Id);
        Assert.Equal(email.Id, storedMessage.EmailId);
        Assert.Equal("foo@host.com", storedMessage.EmailAddress.Value);
        Assert.False(storedMessage.SendDt.HasValue);
        Assert.Equal(initialSelection,storedSending.Selection);
    }

    
}