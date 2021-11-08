﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

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
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<SimulatorHostedService>();
                    services.AddSingleton<IFoodGenerator, FoodGenerator>();
                    services.AddSingleton<INameGenerator, NameGenerator>();
                    services.AddSingleton<IWormBehaviourProvider, WormBehaviourProvider>();
                    services.AddSingleton<ISimulator, Simulator>();
                })
                .ConfigureLogging(context =>
                {
                    context.ClearProviders();
                    context.AddNLog();
                });
        }
    }
}