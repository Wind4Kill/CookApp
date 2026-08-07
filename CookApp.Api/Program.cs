using System.Diagnostics;
using System.Reflection;
using CookApp.Api;
using CookApp.Api.HelpClasses;
using CookApp.Data;
using CookApp.Model;
using CookApp.Model.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opts =>
{
    opts.ReturnHttpNotAcceptable = true;
});
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
if (builder.Environment.IsProduction())
{
    builder.Services.AddStackExchangeRedisOutputCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
        options.InstanceName = "CookApi-cache";
    });

}
builder.Services.AddOutputCache();

builder.Services.AddAutoMapper(conf =>
{
    conf.AddMaps(Assembly.GetAssembly(typeof(Recipe)));
});


string? connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(connectionString, options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null));
    if (builder.Environment.IsDevelopment())
    {
        options.LogTo(message => Debug.WriteLine(message)).
        EnableDetailedErrors().
        EnableSensitiveDataLogging();
    }
});

builder.Services.AddServices();

if (builder.Environment.IsDevelopment() || builder.Environment.IsProduction())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler();
    app.MigrateDb();
}

app.UseStatusCodePages();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SeedData();
}

app.UseHttpsRedirection();
app.UseOutputCache();
app.MapControllers();

app.Run();


