using Newtonsoft.Json;
using TagsAPI.Services.Interfaces;

namespace TagsAPI.StartupTasks.SynchronizeTags
{
    public class SynchronizeTagsStartupTask(IServiceProvider serviceProvider, ILogger<SynchronizeTagsStartupTask> logger) : IStartupTask
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private readonly ILogger<SynchronizeTagsStartupTask> logger = logger;

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
            var tagsService = scope.ServiceProvider.GetRequiredService<ITagsService>();
            var result = await tagsService.Synchronize();

            logger.LogInformation("Synchronization result: {SynchronizationResult}", JsonConvert.SerializeObject(result));
        }
    }
}
