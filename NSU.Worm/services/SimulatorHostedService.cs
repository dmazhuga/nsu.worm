using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NSU.Worm
{
    public class SimulatorHostedService : IHostedService
    {
        private bool _running = true;

        private ISimulator _simulator;

        public SimulatorHostedService(ISimulator simulator)
        {
            _simulator = simulator;
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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;
            return Task.CompletedTask;  
        }
    }
}