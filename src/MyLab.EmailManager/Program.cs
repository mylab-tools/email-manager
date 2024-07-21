using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager;
using MyLab.EmailManager.App.ConfirmationStuff;
using MyLab.EmailManager.App.Features;
using MyLab.EmailManager.App.Mapping;
using MyLab.EmailManager.BackgroundSending;
using MyLab.EmailManager.Confirmations;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Emails;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.EmailManager.Infrastructure.MailServer;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.EmailManager.Infrastructure.Messaging;
using MyLab.EmailManager.Infrastructure.Repositories;
using MyLab.EmailManager.Sendings;
using MyLab.Log;
using MyLab.WebErrors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var srv = builder.Services;

srv.AddControllers(opt => opt.AddExceptionProcessing())
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<ConfirmationStep>(JsonNamingPolicy.KebabCaseLower));
    });
srv.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MyLab.EmailManager.App.Anchor>());
srv.AddAutoMapper(e =>
{
    e.AddProfile(typeof(EmailDtoMappingProfile));
    e.AddProfile(typeof(EmailMappingProfile));
    e.AddProfile(typeof(ConfirmationMappingProfile));
    e.AddProfile(typeof(ConfirmationStateDtoMappingProfile));
    e.AddProfile(typeof(SendingDtoMappingProfile));
    e.AddProfile(typeof(SendingMappingProfile));
});

srv.AddScoped<SendingMappingProfile.DbMessageSendingStatusValueResolver>();
srv.AddScoped<SendingMappingProfile.DbSendingSendingStatusValueResolver>();

srv.AddScoped<IEmailRepository, EmailRepository>()
    .AddScoped<IConfirmationRepository, ConfirmationRepository>()
    .AddScoped<ISendingRepository, SendingRepository>()
    .AddSingleton<IMessageTemplateService, MessageTemplateService>()
    .AddSingleton<IMessageTemplateProvider, MessageTemplateProvider>()
    .AddSingleton<IMailServerIntegration, MailServerIntegration>()
    .AddSingleton<IMailMessageSender, MailMessageSender>()
    .AddSingleton<ConfirmationMessageSender>()
    .AddScoped<EmailCreationLogic>()
    .AddHostedService<BgSendingService>();

srv.AddLogging(l => l.AddMyLabConsole());

srv.AddOptions<EmailManagerOptions>()
    .BindConfiguration("EmailManager")
    .ValidateDataAnnotations()
    .ValidateOnStart();

srv.AddOptions<TemplateOptions>()
    .BindConfiguration("EmailManager/Templates")
    .ValidateDataAnnotations()
    .ValidateOnStart();

srv.AddOptions<SmtpOptions>()
    .BindConfiguration("EmailManager/Smtp")
    .ValidateDataAnnotations()
    .ValidateOnStart();

srv.AddOptions<ConfirmationOptions>()
    .BindConfiguration("EmailManager/Confirmation")
    .ValidateDataAnnotations()
    .ValidateOnStart();

InitDb(srv);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

void InitDb(IServiceCollection srvCollection)
{
    var connectionString = builder.Configuration.GetConnectionString("db");
    Action<DbContextOptionsBuilder> tuneDbOpts = opt => opt.UseMySql
    (
        connectionString,
        new MySqlServerVersion("8.0.0")
    );

    srvCollection.AddDbContext<DomainDbContext>(tuneDbOpts);
    srvCollection.AddDbContext<ReadDbContext>(tuneDbOpts);
}

public partial class Program { }