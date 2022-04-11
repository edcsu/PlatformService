using Microsoft.EntityFrameworkCore;
using PlatformService.Business.Platform.Repositories.Implementations;
using PlatformService.Business.Platform.Repositories.Interfaces;
using PlatformService.Data;
using PlatformService.Data.Context;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.SqlServer.Destructurers;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var logger = new LoggerConfiguration()
        .Enrich.WithThreadId()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .Enrich.WithThreadName()
        .Enrich.WithClientAgent()
        .Enrich.WithClientIp()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithExceptionDetails(
            new DestructuringOptionsBuilder()
            .WithDefaultDestructurers()
            .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
            .WithDestructurers(new[] { new SqlExceptionDestructurer() }))
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateBootstrapLogger();

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog(logger);

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseInMemoryDatabase("inmem"));

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Seed.PopulateDb(app);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}