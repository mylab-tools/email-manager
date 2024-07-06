using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MyLab.EmailManager.Domain.Repositories;
using MyLab.EmailManager.Emails;
using MyLab.EmailManager.Infrastructure.Db;
using MyLab.EmailManager.Infrastructure.Db.EfModels;
using MyLab.EmailManager.Infrastructure.Repositories;
using MyLab.WebErrors;

var builder = WebApplication.CreateBuilder(args);

// AddAsync services to the container.

var srv = builder.Services;

srv.AddControllers(opt => opt.AddExceptionProcessing());
srv.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MyLab.EmailManager.App.Anchor>());
srv.AddAutoMapper(e => e.AddProfile(typeof(EmailDefDtoMappingProfile)));
srv.AddScoped<IEmailRepository, EmailRepository>();

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