using System.Diagnostics;
using System.Reflection;
using CookApp.Api;
using CookApp.Api.HelpClasses;
using CookApp.Application;
using CookApp.Application.MapProfiles;
using CookApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opts =>
{
    opts.ReturnHttpNotAcceptable = true;
});
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
if(builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
if (builder.Environment.IsProduction())
{
    string redisConnectionString = builder.Configuration.GetConnectionString("RedisConnectionString")!;
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "CookApi-cache";
    });
    builder.Services.AddStackExchangeRedisOutputCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "CookApi-cache";
    });

}
builder.Services.AddOutputCache();

builder.Services.AddAutoMapper(conf =>
{
    conf.AddMaps(typeof(RecipeProfile).Assembly);
});


builder.Services.AddApplication();

string? connectionString = builder.
Configuration.GetConnectionString("DevelopmentConnectionString");
builder.Services.AddData(connectionString);

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


