using TagsAPI.Exceptions;

namespace TagsAPI.StartupTasks.Extensions
{
    public static class StartupTaskWebHostExtensions
    {
        public static async Task RunWithTasksAsync(this WebApplication app, CancellationToken cancellationToken = default)
        {
            await RunStartupTasks(app, cancellationToken);
            await app.RunAsync(cancellationToken);
        }

        private static async Task RunStartupTasks(WebApplication app, CancellationToken cancellationToken)
        {
            try
            {
                var startupTasks = app.Services.GetServices<IStartupTask>();

                foreach (var startupTask in startupTasks)
                {
                    await startupTask.ExecuteAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                var message = $"Startup task message: {e.Message}";
                throw new StartupTaskException(message, e.StackTrace!);
            }
        }
    }
}
