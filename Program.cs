using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RuokaAppiBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Sallii kaikki yhteydet (esim. html)
builder.Services.AddCors(options =>
{
    options.AddPolicy("salliKaikki",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

// ------Connection string luetaan app settings.json tiedostosta--------------

builder.Services.AddDbContext<kauppalistadbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("azure")
    ));

//// ------------- tuodaan appSettings.jsoniin tekem‰mme AppSettings m‰‰ritys ------------
//var appSettingsSection = builder.Configuration.GetSection("AppSettings");
//builder.Services.Configure<AppSettings>(appSettingsSection);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
