using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NSU.Worm
{
    public class SimulatorHostedService : IHostedService
    {
        private IHostApplicationLifetime _appLifetime;

        private ISimulator _simulator;

        public SimulatorHostedService(ISimulator simulator, IHostApplicationLifetime appLifetime)
        {
            _simulator = simulator;
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(RunAsync, cancellationToken);
            return Task.CompletedTask;
        }

        private void RunAsync()
        {
            Thread.Sleep(250);
            _simulator.Start();
            _appLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;  
        }
    }
}