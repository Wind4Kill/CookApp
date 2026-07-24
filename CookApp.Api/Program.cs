using System.Diagnostics;
using CookApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();


string? connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(connectionString, options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null));
    if(builder.Environment.IsDevelopment())
    {
        options.LogTo(message => Debug.WriteLine(message)).
        EnableDetailedErrors().
        EnableSensitiveDataLogging();
    }
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


