using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TagsAPI;
using TagsAPI.DataAccess;
using TagsAPI.DataAccess.Repositories;
using TagsAPI.Services;
using TagsAPI.Services.Interfaces;
using TagsAPI.StartupTasks.AddMigrations;
using TagsAPI.StartupTasks.Extensions;
using TagsAPI.StartupTasks.SynchronizeTags;
using TagsAPI.Validators.Common;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services!;

services.AddDbContext<TagsDbContext>(options =>
                options.UseSqlServer(Config.Database.ConnectionString(builder.Configuration)));

services.RegisterGenericTypes(
    typeof(IRepository<,>),
    typeof(IRequestValidator<>));

services.RegisterMarkerTypes(typeof(IService));

services.AddScoped(typeof(IStackOverflowAPIService<>), typeof(StackOverflowAPIService<>));

services.AddStartupTask<AddMigrationsStartupTask>();
services.AddStartupTask<SynchronizeTagsStartupTask>();

services.AddCors();

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TagsAPI",
        Version = "v1",
        Description = "Web API for StackOverflow Tags",
    });
});

services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tags v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

await app.RunWithTasksAsync();