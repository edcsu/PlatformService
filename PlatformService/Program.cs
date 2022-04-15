using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Business.Platform.Config;
using PlatformService.Business.Platform.Repositories.Implementations;
using PlatformService.Business.Platform.Repositories.Interfaces;
using PlatformService.Business.Platform.Services.PlatformService;
using PlatformService.Core;
using PlatformService.Data;
using PlatformService.Data.Context;
using PlatformService.DataServices.CommandService;
using Serilog;
using Serilog.Debugging;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.SqlServer.Destructurers;

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    SelfLog.Enable(Console.WriteLine);

    Log.Logger = new LoggerConfiguration()
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
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateBootstrapLogger();

    var seqConfig = builder.Configuration.GetSeqSettings();

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .WriteTo.Seq(seqConfig.Url)
        .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

    builder.Services.AddScoped<IPlatformService, PlatformService.Business.Platform.Services.PlatformService.PlatformService>();

    builder.Services.Configure<CommandServiceConfig>(options => builder.Configuration
                .GetSection(CommandServiceConfig.ConfigurationName)
                .Bind(options));

    // Inject Command Client
    builder.Services.AddHttpClient<CommandClient>()
        .AddPolicyHandler(PollyPolicies.GetRetryPolicy())
        .AddPolicyHandler(PollyPolicies.GetCircuitBreakerPolicy());

    builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

    var isProd = builder.Environment.IsProduction();
    if (isProd)
    {
        Log.Information("Using in sqlserver db");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
    else
    {
        Log.Information("Using in memory db");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
         options.UseInMemoryDatabase("inmem"));
    }

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
    
    app.UseGlobalErrorHandler();

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Seed.PopulateDb(app, isProd);

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