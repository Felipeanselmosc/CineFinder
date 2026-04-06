using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CineFinder.Infrastructure.Repositories;
using CineFinder.Application.Interfaces;
using CineFinder.Application.Services;
using CineFinder.Domain.Interfaces;
using CineFinder.Infrastructure.Data.Context;
using System.Text.Json.Serialization;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(path: "logs/cinefinder-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Iniciando CineFinder API...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; options.JsonSerializerOptions.WriteIndented = true; });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CineFinder API", Version = "v1" }); });
    builder.Services.AddDbContext<CineFinderDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddCors(options => { options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }); });
    builder.Services.AddHealthChecks()
        .AddSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!, name: "sqlserver", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "sql", "sqlserver" })
        .AddDbContextCheck<CineFinderDbContext>(name: "dbcontext", failureStatus: HealthStatus.Degraded, tags: new[] { "db", "ef" });
    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService("CineFinder.API"))
        .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation().AddEntityFrameworkCoreInstrumentation().AddHttpClientInstrumentation().AddConsoleExporter())
        .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation().AddConsoleExporter());
    builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
    builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
    builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
    builder.Services.AddScoped<IListaRepository, ListaRepository>();
    builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
    builder.Services.AddScoped<IUsuarioService, UsuarioService>();
    builder.Services.AddScoped<IFilmeService, FilmeService>();
    builder.Services.AddScoped<IGeneroService, GeneroService>();
    builder.Services.AddScoped<IListaService, ListaService>();
    builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
    var app = builder.Build();
    if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CineFinder API v1"); c.RoutePrefix = string.Empty; }); app.UseDeveloperExceptionPage(); }
    else { app.UseExceptionHandler("/Error"); app.UseHsts(); }
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseRouting();
    app.UseAuthorization();
    app.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = async (context, report) => { var result = JsonSerializer.Serialize(new { status = report.Status.ToString(), duration = report.TotalDuration, checks = report.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString(), description = e.Value.Description, duration = e.Value.Duration, exception = e.Value.Exception == null ? null : e.Value.Exception.Message }) }); context.Response.ContentType = MediaTypeNames.Application.Json; await context.Response.WriteAsync(result); } });
    app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
    app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("db") });
    app.MapControllers();
    Log.Information("CineFinder API iniciada com sucesso.");
    app.Run();
}
catch (Exception ex) { Log.Fatal(ex, "A aplicacao falhou ao iniciar."); }
finally { Log.CloseAndFlush(); }

public partial class Program { }
