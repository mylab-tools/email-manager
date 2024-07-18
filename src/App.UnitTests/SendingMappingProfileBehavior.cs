using AutoMapper;
using MyLab.EmailManager.App.Mapping;
using MyLab.EmailManager.App.ViewModels;
using MyLab.EmailManager.Infrastructure.Db.EfModels;

namespace App.UnitTests
{
    public class SendingMappingProfileBehavior
    {
        [Fact]
        public void ShouldMapDbToViewModel()
        {
            //Arrange
            var dbMessage = new DbMessage
            {
                Id = Guid.NewGuid(),
                EmailId = Guid.NewGuid(),
                EmailAddress = "foo@host.com",
                Content = "bar",
                CreateDt = DateTime.Now,
                IsHtml = true,
                SendDt = DateTime.Now,
                SendingId = Guid.NewGuid(),
                Title = "baz"
            };

            var dbSending = new DbSending
            {
                Id = Guid.NewGuid(),
                Selection = "[{ \"Name\": \"foo\", \"Value\": \"bar\" }]",
                SimpleContent = "baz",
                TemplateId = "qoz",
                TemplateArgs = "{ \"baz\": \"qoz\" }",
                Messages = new List<DbMessage> { dbMessage }
            };

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<SendingMappingProfile>());
            var mapper = mapperConfig.CreateMapper();

            //Act
            var vmSending = mapper.Map<SendingViewModel>(dbSending);

            //Assert
            Assert.NotNull(vmSending);
            Assert.Equal("baz", vmSending.SimpleContent);
            Assert.Equal("qoz", vmSending.TemplateId);
            Assert.Equal(dbSending.Id, vmSending.Id);
            Assert.Single(vmSending.Selection);
            Assert.Contains(vmSending.Selection, s => s is { Key: "foo", Value: "bar" });
            Assert.NotNull(vmSending.TemplateArgs);
            Assert.Single(vmSending.TemplateArgs);
            Assert.Contains(vmSending.TemplateArgs, s => s is { Key: "baz", Value: "qoz" });
            Assert.NotNull(vmSending.Messages);
            Assert.Single(vmSending.Messages);

            var vmMessage = vmSending.Messages.First();

            Assert.Equal("foo@host.com", vmMessage.EmailAddress);
            Assert.Equal("bar", vmMessage.Content);
            Assert.Equal("baz", vmMessage.Title);
            Assert.True(vmMessage.IsHtml);
            Assert.Equal(dbMessage.Id, vmMessage.Id);
            Assert.Equal(dbMessage.EmailId, vmMessage.EmailId);
            Assert.NotEqual(default, vmMessage.CreateDt);
            Assert.True(vmMessage.SendDt.HasValue);
        }
    }
}
