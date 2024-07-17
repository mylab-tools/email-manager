using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyLab.EmailManager.Client.Sendings;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace EmailManager.FuncTests
{
    public partial class SendingsBehavior
    {
        [Fact]
        public async Task ShouldCreateSending()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var emailId = await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "bar");
            await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "qoz");
            var client = clientAsset.ApiClient;

            var dbCtx = clientAsset.ServiceProvider.GetRequiredService<ReadDbContext>();

            //Act
            var sendingId = await client.CreateAsync
            (
                new SendingDefDto
                (
                    new Dictionary<string, string>
                    {
                        {"marker1", "foo"},
                        {"marker2", "bar"}
                    },
                    "Hello!",
                    "The sun is yellow",
                    null,
                    null
                )
            );

            var sending = await dbCtx.Sendings
                .Include(dbSending => dbSending.Messages)
                .FirstOrDefaultAsync(s => s.Id == sendingId);

            //Assert
            Assert.NotNull(sending);
            Assert.NotNull(sending.Selection);
            Assert.Null(sending.TemplateId);
            Assert.Null(sending.TemplateArgs);
            Assert.Equal("The sun is yellow", sending.SimpleContent);
            Assert.NotNull(sending.Messages);
            Assert.Single(sending.Messages);

            var foundMessage = sending.Messages.First();

            Assert.True(foundMessage is
            {
                Title:"Hello!",
                Content: "The sun is yellow",
                IsHtml: false,
                Address: "foo@bar.com",
            });
            Assert.Equal(emailId, foundMessage.EmailId);
            Assert.NotEqual(default, foundMessage.CreateDt);
            Assert.False(foundMessage.SendDt.HasValue);
        }

        [Fact]
        public async Task ShouldGetSending()
        {
            //Arrange
            var clientAsset = _apiFxt.StartWithProxy();
            var emailId = await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "bar");
            await CreateAndConfirmEmail(clientAsset.ServiceProvider, "foo", "qoz");
            var client = clientAsset.ApiClient;

            var sendingId = await client.CreateAsync
            (
                new SendingDefDto
                (
                    new Dictionary<string, string>
                    {
                        {"marker1", "foo"},
                        {"marker2", "bar"}
                    },
                    "Hello!",
                    "The sun is yellow",
                    null,
                    null
                )
            );

            //Act

            var sending = await client.GetAsync(sendingId);

            //Assert
            Assert.NotNull(sending);
            Assert.NotNull(sending.Selection);
            Assert.Null(sending.TemplateId);
            Assert.Null(sending.TemplateArgs);
            Assert.Equal("The sun is yellow", sending.SimpleContent);
            Assert.NotNull(sending.Messages);
            Assert.Single(sending.Messages);

            var foundMessage = sending.Messages.First();

            Assert.True(foundMessage is
            {
                Title: "Hello!",
                Content: "The sun is yellow",
                IsHtml: false
            });
            Assert.Equal(emailId, foundMessage.EmailId);
            Assert.NotEqual(default, foundMessage.CreateDt);
            Assert.False(foundMessage.SendDt.HasValue);
        }
    }
}
