using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
    }
}
