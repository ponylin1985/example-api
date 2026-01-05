using Example.Api.Data;
using Example.Api.DateTimeOffsetProviders;
using Example.Api.Endpoints;
using Example.Api.Extensions;
using Example.Api.Infrastructure;
using Example.Api.Options;
using Example.Api.Repositories;
using Example.Api.Services;
using Example.Api.Validators;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Settings.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    var options = new ConfigurationReaderOptions(
        typeof(ConsoleLoggerConfigurationExtensions).Assembly,
        typeof(FileLoggerConfigurationExtensions).Assembly,
        typeof(CompactJsonFormatter).Assembly
    );

    configuration
        .ReadFrom.Configuration(context.Configuration, options)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentUserName();
});

builder.Services.AddOptions(builder.Configuration);
builder.Services.AddCacheOptions(builder.Configuration);
builder.Services.AddJsonSerializationOptions();
builder.Services.AddDateTimeOffsetProviders();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddInfrastructures(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseTraceId();
app.UseRequestResponseLogging();

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.InjectStylesheet("/swagger-dark.css");
    });
    app.MapOpenApi();
}

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();

app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }))
    .WithName("HealthCheck")
    .WithTags("Health");

app.MapApiEndpoints();
app.Run();
