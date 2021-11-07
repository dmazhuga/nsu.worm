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
            Task.Run(() => _simulator.Start());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;
            return Task.CompletedTask;  
        }
    }
}