using System.Diagnostics;
using System.Reflection;
using CookApp.Api.HelpClasses;
using CookApp.Data;
using CookApp.Model;
using CookApp.Model.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCaching();
builder.Services.AddControllers(opts =>
{
    opts.ReturnHttpNotAcceptable = true;
});
builder.Services.AddProblemDetails();
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

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler(errorApp =>
      {
          errorApp.Run(async context =>
          {
              var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
              if (error is null)
                  return;

              var (statusCode, message) = error switch

              {
                  EntityNotFoundException => (StatusCodes.Status404NotFound, error.Message),
                  _ => (StatusCodes.Status500InternalServerError, error.Message)
              };

              var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
              {
                  Status = statusCode,
                  Title = message,
                  Type = $"https://httpstatuses.com/{statusCode}",
                  Detail = error.Message,
                  Instance = context.Request.Path
              };

              context.Response.StatusCode = statusCode;

              await context.Response.WriteAsJsonAsync(problemDetails);
          });
      });
    app.MigrateDb();
}

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SeedData();
}

app.UseHttpsRedirection();
app.UseResponseCaching();
app.MapControllers();

app.Run();


