using System.Data;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using CookApp.Api;
using CookApp.Api.HelpClasses;
using CookApp.Api.Validators;
using CookApp.Application;
using CookApp.Application.MapProfiles;
using CookApp.Data;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opts =>
{
    opts.ReturnHttpNotAcceptable = true;
});
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
if (builder.Environment.IsDevelopment())
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

builder.Services.AddValidatorsFromAssembly(typeof(UserRegistrationValidator).Assembly);

builder.Services.AddAutoMapper(conf =>
{
    conf.AddMaps(typeof(RecipeProfile).Assembly);
});


builder.Services.AddApplication(builder.Configuration);

string? connectionString = builder.
Configuration.GetConnectionString("DevelopmentConnectionString");
builder.Services.AddData(connectionString);

if (builder.Environment.IsDevelopment() || builder.Environment.IsProduction())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters.ValidateLifetime = true;
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ValidateAudience = true;
    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
    options.TokenValidationParameters.ValidIssuer = builder.Configuration["JwtSettings:Issuer"];
    options.TokenValidationParameters.ValidAudience = builder.Configuration["JwtSettings:Issued"];
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!));
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ForAdmin", policy => policy.
    RequireClaim("name",builder.Configuration["AdminCredentials:Login"]!)
    .RequireClaim("role", "Admin"));
});

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler();
    await app.MigrateDb();
}

app.UseStatusCodePages();
app.UseRouting();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.SeedData();
    await app.AddAdmin();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseOutputCache();
app.MapControllers();

app.Run();


