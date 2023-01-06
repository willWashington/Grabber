using Grabber.Entry;
using Grabber.Utilities;

///Grabber is a worker service responsible for reaching out and grabbing information about keywords fed to it from various internet sources.
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
            var count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                //report heartbeat every 3 seconds
                if (count == 3)
                {
                    count = 0;
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                
                //entry every second
                EntryPoint.Entry();
                count++;

                //define worker service tickrate as delay (aka iterate every second)
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}