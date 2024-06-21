using MyLab.EmailManager.Domain.Dto;
using MyLab.EmailManager.Domain.Entities;
using MyLab.EmailManager.Domain.Exceptions;
using MyLab.EmailManager.Domain.ValueObjects;

namespace Infrastructure.UnitTests;

public partial class AppDbContextBehavior
{
    [Theory]
    [MemberData(nameof(GetInvalidSendMessageDefCases))]
    public void ShouldNotCreateInvalidSending(SendMessageDef? invalidMessageDef)
    {
        //Arrange
        Exception? creationException = null;

        //Act
        try
        {
            new Sending(Guid.NewGuid(), invalidMessageDef);
        }
        catch (Exception e)
        {
            creationException = e;
        }

        //Assert
        Assert.NotNull(creationException);
        Assert.Equal
        (
            invalidMessageDef == null, 
            creationException.GetType() == typeof(ArgumentNullException)
        );
        Assert.Equal
        (
            invalidMessageDef != null,
            creationException.GetType() == typeof(DomainException)
        );
    }

    [Fact]
    public void ShouldStoreSimpleSending()
    {
        //Arrange
        var initialSelection = new EmailLabel[]
        {
            new EmailLabel("baz", "qoz")
        };
        var newSending = new Sending
        (
            Guid.NewGuid(),
            new SendMessageDef
            {
                Title = "foo",
                SimpleContent = new SimpleMessageContent("bar"),
                Selection = initialSelection
            }
        );

        //Act
        _dbContext.Sendings.Add(newSending);
        _dbContext.SaveChanges();

        var storedSending = _dbContext.Sendings
            .FirstOrDefault(s => s.Id == newSending.Id);

        //Assert
        Assert.NotNull(storedSending);
        Assert.NotNull(storedSending.SimpleContent);
        Assert.Null(storedSending.GenericContent);
        Assert.Equal("foo",storedSending.Title.Text);
        Assert.Equal(initialSelection,storedSending.Selection);
        Assert.Equal("bar", storedSending.SimpleContent.Text);
    }

    [Fact]
    public void ShouldStoreGenericSending()
    {
        //Arrange
        var initialSelection = new EmailLabel[]
        {
            new EmailLabel("baz", "qoz")
        };
        var initialPatternArgs = new Dictionary<string, string>
        {
            { "fookey", "barvalue" }
        };

        var newSending = new Sending
        (
            Guid.NewGuid(),
            new SendMessageDef
            {
                Title = "foo",
                GenericContent = new GenericMessageContent
                    (
                        "bar",
                        initialPatternArgs.ToDictionary
                        (
                            t => new FilledString(t.Key),
                            t => t.Value
                        )
                    ),
                Selection = initialSelection
            }
        );

        //Act
        _dbContext.Sendings.Add(newSending);
        _dbContext.SaveChanges();

        var storedSending = _dbContext.Sendings
            .FirstOrDefault(s => s.Id == newSending.Id);

        //Assert
        Assert.NotNull(storedSending);
        Assert.NotNull(storedSending.GenericContent);
        Assert.Null(storedSending.SimpleContent);
        Assert.Equal("foo", storedSending.Title.Text);
        Assert.Equal(initialSelection, storedSending.Selection);
        Assert.Equal("bar", storedSending.GenericContent.PatternId.Text);
        Assert.Equal(initialPatternArgs, storedSending.GenericContent.Args);
    }

    public static object[][] GetInvalidSendMessageDefCases()
    {
        var validSelection = new EmailLabel[] { new("foo", "bar") };
        var simpleContent = new SimpleMessageContent("bar");
        var genericContent = new GenericMessageContent("baz", new Dictionary<FilledString, string>());

        return new object[][]
        {
            new object[]
            {
                null
            },
            new object[]
            {
                new SendMessageDef
                {
                    Title = "foo",
                    Selection = Array.Empty<EmailLabel>(),
                    SimpleContent = simpleContent
                }
            },
            new object[]
            {
                new SendMessageDef
                {
                    Title = "foo",
                    Selection = validSelection,
                    SimpleContent = null,
                    GenericContent = null
                }
            },
            new object[]
            {
                new SendMessageDef
                {
                    Title = "foo",
                    Selection = validSelection,
                    SimpleContent = simpleContent,
                    GenericContent = genericContent
                }
            }
        };
    }
}