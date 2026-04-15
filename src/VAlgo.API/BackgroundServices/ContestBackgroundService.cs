using VAlgo.Modules.Contests.Application.Jobs;

namespace VAlgo.API.BackgroundServices
{
    public sealed class ContestBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ContestBackgroundService> _logger;

        public ContestBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<ContestBackgroundService> logger
        )
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Contest Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var autoStartJob = scope.ServiceProvider.GetRequiredService<AutoStartContestJob>();
                    var autoFinishJob = scope.ServiceProvider.GetRequiredService<AutoFinishContestJob>();

                    await autoStartJob.Execute(stoppingToken);
                    await autoFinishJob.Execute(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in ContestBackgroundService");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}