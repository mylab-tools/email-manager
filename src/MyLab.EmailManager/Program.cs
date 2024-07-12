using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyLab.EmailManager.App.Mapping;
using MyLab.EmailManager.Confirmations;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Emails;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.EmailManager.Infrastructure.MessageTemplates;
using MyLab.EmailManager.Infrastructure.Repositories;
using MyLab.EmailManager.Options;
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
});
srv.AddScoped<IEmailRepository, EmailRepository>();
srv.AddScoped<IConfirmationRepository, ConfirmationRepository>();
srv.AddScoped<IMessageTemplateService, MessageTemplateService>();
srv.AddScoped<IMessageTemplateProvider, MessageTemplateProvider>(sp =>
{
    var opts = sp.GetRequiredService<IOptions<EmailManagerOptions>>();
    return new MessageTemplateProvider(opts.Value.TemplatePath);
});

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