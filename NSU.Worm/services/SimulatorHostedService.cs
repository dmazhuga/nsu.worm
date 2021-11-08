using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NSU.Worm
{
    public class SimulatorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<SimulatorHostedService> _logger;

        private readonly ISimulator _simulator;

        public SimulatorHostedService(ISimulator simulator, IHostApplicationLifetime appLifetime,
            ILogger<SimulatorHostedService> logger)
        {
            _simulator = simulator;
            _appLifetime = appLifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(RunAsync, cancellationToken);
            return Task.CompletedTask;
        }

        private void RunAsync()
        {
            Thread.Sleep(250);
            try
            {
                _simulator.Start();
            }
            catch (Exception ex)
            {
                _logger.LogError("Simulation stopped with exception: ${ex}", ex);
            }
            finally
            {
                _appLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}