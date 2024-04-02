using Microsoft.EntityFrameworkCore;
using TagsAPI.DataAccess;

namespace TagsAPI.StartupTasks.AddMigrations
{
    public class AddMigrationsStartupTask(IServiceProvider serviceProvider) : IStartupTask
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TagsDbContext>()
                ?? throw new NullReferenceException();

            await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
        }
    }
}
