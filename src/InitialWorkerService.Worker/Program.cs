using InitialWorkerService;
using InitialWorkerService.Contracts.Logs;
using InitialWorkerService.Domain;
using InitialWorkerService.Repository.EFCore;
using InitialWorkerService.Services.Logs;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(@"../../LocalLogs/LogFile.txt")
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            c => c.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

        services
            .AddTransient(d => new AppDbContext(optionBuilder.Options))
            .AddTransient<IUnitOfWork, UnitOfWork>()
            .AddTransient<ILogsService, LogsService>()
            .AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

try
{
    using (var serviceScope = host.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        Log.Information("Running DB Migration Started...");

        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        Log.Information("DB Migration Complete...");
    }

    Log.Information("Starting the service...");
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting this service!");
    return;
}
finally
{
    Log.CloseAndFlush();
}
