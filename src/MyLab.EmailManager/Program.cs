using MyLab.WebErrors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var srv = builder.Services;

srv.AddControllers(opt => opt.AddExceptionProcessing());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
