using System;
using System.Threading.Tasks;
using Evo.Infrastructure.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentry;
using Serilog;
using Serilog.Events;

namespace Evo.Scm;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
#else
            .MinimumLevel.Error()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
#endif
            .Enrich.FromLogContext()
            //.WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
            .WriteTo.Async(c => c.Console())
            
#endif
            .CreateLogger();

        try
        {
            Log.Information("Starting Evo.Scm.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
#if !DEBUG
            builder.WebHost.UseSentry(options =>
            {
                options.AddEntityFramework();
            }); //debug模式下不上报异常到sentry
#endif
            builder.Host
                .AddAppSettingsSecretsJson()
                .AddConsulConfiguration("scm")
                .UseAutofac()
#if DEBUG
                .UseSerilog()
#endif
                ;
           
            await builder.AddApplicationAsync<ScmHttpApiHostModule>();  
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
