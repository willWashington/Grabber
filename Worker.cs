using Grabber.Entry;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                EntryPoint.Entry();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}