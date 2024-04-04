using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TagsAPI.DataAccess;
using TagsAPI.StartupTasks;
using TagsAPI.StartupTasks.BackgroundServices;

namespace TagsAPI.IntegrationTests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory, string testName)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContext = services
                            .FirstOrDefault(sd => sd.ServiceType == typeof(DbContextOptions<TagsDbContext>));
                        if (dbContext is not null)
                        {
                            services.Remove(dbContext);
                        }
                        services.AddDbContext<TagsDbContext>(
                            options =>
                            {
                                options.UseInMemoryDatabase(testName);
                                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                            });

                        var startupTasks = services.Where(sd => sd.ServiceType == typeof(IStartupTask)).ToList();
                        if (!startupTasks.IsNullOrEmpty())
                        {
                            foreach (var startupTask in startupTasks)
                            {
                                services.Remove(startupTask);
                            }
                        }

                        var synchronizeTagsBackgroundService = services
                            .FirstOrDefault(sd => sd.ImplementationType == typeof(SynchronizeTagsBackgroundService));
                        if (synchronizeTagsBackgroundService is not null)
                        {
                            services.Remove(synchronizeTagsBackgroundService);
                        }

                        services.AddMvc(options =>
                        {
                            options.Filters.Add(new AllowAnonymousFilter());
                        }).AddApplicationPart(typeof(Program).Assembly);
                    });
                });

            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/"); // :( ???
        }
    }

    public class IntegrationTests<TestType>(WebApplicationFactory<Program> factory)
        : IntegrationTests(factory, $"{typeof(TestType).Name}_{Guid.NewGuid()}")
    {
    }
}
