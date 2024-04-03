using Newtonsoft.Json;
using TagsAPI.Services.Interfaces;

namespace TagsAPI.StartupTasks.BackgroundServices
{
    public class SynchronizeTagsBackgroundService(IServiceProvider serviceProvider, ILogger<SynchronizeTagsBackgroundService> logger) : BackgroundService
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private readonly ILogger<SynchronizeTagsBackgroundService> logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var tagsService = scope.ServiceProvider.GetRequiredService<ITagsService>();
                var result = await tagsService.Synchronize();

                logger.LogInformation("Synchronization result: {SynchronizationResult}", JsonConvert.SerializeObject(result));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
            }
        }
    }
}
