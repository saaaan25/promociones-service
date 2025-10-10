namespace promociones.Services
{
    public class PeriodicSyncService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<PeriodicSyncService> _logger;
        private readonly IConfiguration _config;
        private readonly TimeSpan _interval;

        public PeriodicSyncService(IServiceProvider provider, ILogger<PeriodicSyncService> logger, IConfiguration config)
        {
            _provider = provider;
            _logger = logger;
            _config = config;

            var minutes = _config.GetValue<int?>("Sync:IntervalMinutes") ?? 30;
            if (minutes <= 0) minutes = 30;
            _interval = TimeSpan.FromMinutes(minutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PeriodicSyncService started. Interval: {Interval}", _interval);

            await RunOnceAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (OperationCanceledException) { break; }

                await RunOnceAsync(stoppingToken);
            }

            _logger.LogInformation("PeriodicSyncService stopping.");
        }

        private async Task RunOnceAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _provider.CreateScope();
                var sync = scope.ServiceProvider.GetRequiredService<ProductoSyncService>();
                _logger.LogInformation("PeriodicSyncService: starting sync at {time}", DateTimeOffset.UtcNow);
                await sync.SyncListadoAsync();
                _logger.LogInformation("PeriodicSyncService: sync finished at {time}", DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during periodic sync");
            }
        }
    }
}
