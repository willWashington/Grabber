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
            var heartbeatCounter = 0;
            var logCounter = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                //report heartbeat every 3 seconds
                if (logCounter == 3 && !GrabberLogger.Quiet)
                {
                    logCounter = 0;
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                if (heartbeatCounter % 60 == 0 && GrabberLogger.Quiet)
                {
                    GrabberLogger.Quiet = false;
                }
                
                //entry every second
                EntryPoint.Entry();
                heartbeatCounter++;
                logCounter++;
                //define worker service tickrate as delay (aka iterate every second)
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}