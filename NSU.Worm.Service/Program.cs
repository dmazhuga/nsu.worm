using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NSU.Worm.Data;

namespace NSU.Worm
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContextFactory<EnvironmentContext>(options => 
                        options.UseNpgsql("name=ConnectionStrings:EnvironmentDatabase"));
                    
                    services.AddHostedService<SimulatorHostedService>();
                    
                    //services.AddSingleton<IFoodGenerator, RandomFoodGenerator>();
                    services.AddSingleton<IFoodGenerator, PatternFoodGenerator>();
                    services.AddSingleton<INameGenerator, NameGenerator>();
                    services.AddSingleton<IWormBehaviourProvider, WormBehaviourProvider>();
                    services.AddSingleton<IMutableWorldState, WorldState>();
                    services.AddSingleton<ISimulator, Simulator>();

                    services.Configure<SimulatorOptions>(context
                        .Configuration.GetSection(nameof(SimulatorOptions)));
                })
                .ConfigureLogging(context =>
                {
                    context.ClearProviders();
                    context.AddNLog();
                });
        }
    }
}