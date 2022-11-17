using InitialWorkerService;
using InitialWorkerService.Contracts.Logs;
using InitialWorkerService.Domain;
using InitialWorkerService.Repository.EFCore;
using InitialWorkerService.Services.Logs;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using InitialWorkerService.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(@"../../LocalLogs/LogFile.txt")
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder =>
    {
        builder.Configure(app =>
        {
            app.UseRouting();
            app.UseHangfireDashboard();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();
            });
        });
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            c => c.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangFireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        services
            .AddTransient(d => new AppDbContext(optionBuilder.Options))
            .AddTransient<IUnitOfWork, UnitOfWork>()
            .AddTransient<ILogsService, LogsService>()
            .AddTransient<IWorkService, LogMessageService>()
            .AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

try
{
    using (var serviceScope = host.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        Log.Information("DB Migration Starting...");

        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        Log.Information("DB Migration Complete...");
    }

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
