using Microsoft.Extensions.Options;

namespace TraversalCoreProje.Infrastructure
{
    /// <summary>
    /// Setzt die Demo-Datenbank in einem festen Intervall auf den Ausgangszustand zurück,
    /// damit Besucher der öffentlichen Demo immer saubere Daten vorfinden.
    /// </summary>
    public class DemoResetService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly DemoOptions _options;
        private readonly ILogger<DemoResetService> _logger;

        public static DateTime? LastReset { get; private set; }
        public static DateTime? NextReset { get; private set; }

        public DemoResetService(IServiceScopeFactory scopeFactory, IOptions<DemoOptions> options, ILogger<DemoResetService> logger)
        {
            _scopeFactory = scopeFactory;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.Enabled || _options.ResetIntervalHours <= 0) return;

            var interval = TimeSpan.FromHours(_options.ResetIntervalHours);
            while (!stoppingToken.IsCancellationRequested)
            {
                NextReset = DateTime.Now.Add(interval);
                try { await Task.Delay(interval, stoppingToken); }
                catch (TaskCanceledException) { return; }

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var init = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                    await init.InitializeAsync(reset: true);
                    LastReset = DateTime.Now;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Zurücksetzen der Demo-Datenbank fehlgeschlagen.");
                }
            }
        }
    }
}
