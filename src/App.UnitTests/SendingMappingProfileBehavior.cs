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
                Address = "foo@host.com",
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
                Selection = "{ \"foo\": \"bar\" }",
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
            Assert.True(vmSending is
            {
                SimpleContent:"baz",
                TemplateId:"qoz"
            });
            Assert.Equal(dbSending.Id, vmSending.Id);
            Assert.Single(vmSending.Selection);
            Assert.Contains(vmSending.Selection, s => s is {Key: "foo", Value: "bar" });
            Assert.Single(vmSending.TemplateArgs);
            Assert.Contains(vmSending.TemplateArgs, s => s is { Key: "baz", Value: "qoz" });
            Assert.NotNull(vmSending.Messages);
            Assert.Single(vmSending.Messages);

            var vmMessage = vmSending.Messages.First();

            Assert.True(vmMessage is
            {
                EmailAddress: "foo@host.com",
                Content:"bar",
                IsHtml:true,
                Title:"baz"
            });
            Assert.Equal(dbMessage.Id, vmMessage.Id);
            Assert.Equal(dbMessage.EmailId, vmMessage.EmailId);
            Assert.NotEqual(default, vmMessage.CreateDt);
            Assert.True(vmMessage.SendDt.HasValue);
        }
    }
}
