using Grabber.Entry;
using Grabber.Models;
using Grabber.Utilities;
using System.Diagnostics;

namespace Grabber
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                EntryPoint.Entry();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}