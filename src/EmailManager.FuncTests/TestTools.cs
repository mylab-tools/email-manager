using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLab.EmailManager.App.Features.CompleteConfirmation;
using MyLab.EmailManager.App.Features.CreateOrUpdateEmail;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.EmailManager.Infrastructure.MailServer;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.EmailManager.Infrastructure.Messaging;

namespace EmailManager.FuncTests
{
    static class TestTools
    {
        private static readonly InMemoryDatabaseRoot DbRoot = new();

        public static void SetFakeConfiguration(IServiceCollection srv)
        {
            srv.Configure<SmtpOptions>(opt =>
            {
                opt.Host = "foo";
                opt.Login = "bar";
                opt.Password = "baz";
                opt.SenderEmail = "qoz";
            });
        }

        public static void SetupMemoryDb(IServiceCollection srv, DomainDbContext domainDbContext, ReadDbContext readDbContext)
        {   
            srv.AddSingleton(domainDbContext);
            srv.AddSingleton(readDbContext);
        }

        public static void RegisterSilentMailStuff(IServiceCollection srv)
        {
            srv.AddSingleton(Mock.Of<IMailMessageSender>());
        }

        public static async Task<(Guid EmailId, Guid ConfirmationSeed)> CreateEmailAsync(IServiceProvider serviceProvider, string address = "foo@bar.com", IDictionary<string, string>? labels = null)
        {
            var scopedSp = serviceProvider
                .CreateScope()
                .ServiceProvider;

            var testEmailId = Guid.NewGuid();

            await scopedSp.GetRequiredService<IMediator>()
                .Send(new CreateOrUpdateEmailCommand(testEmailId, address, labels));

            var confirmationSeed = await scopedSp
                .GetRequiredService<ReadDbContext>()
                .Confirmations
                .Where(c => c.EmailId == testEmailId)
                .Select(c => c.Seed)
                .FirstAsync();

            return (testEmailId, confirmationSeed);
        }

        public static Task ConfirmEmailAsync(IServiceProvider serviceProvider, Guid confirmationSeed)
        {
            var scopedSp = serviceProvider
                .CreateScope()
                .ServiceProvider;

            return scopedSp.GetRequiredService<IMediator>()
                .Send(new CompleteConfirmationCommand(confirmationSeed));
        }
    }
}
