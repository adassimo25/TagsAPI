namespace TagsAPI.StartupTasks.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<Task>(this IServiceCollection services)
            where Task : class, IStartupTask
        {
            services.AddTransient<IStartupTask, Task>();

            return services;
        }

        public static IServiceCollection RegisterAppsettingsSections(this IServiceCollection services, ConfigurationManager configuration)
        {
            var type = typeof(Config.Sections.Section);
            var assembly = type.Assembly;

            foreach (var sectionType in assembly.GetTypes().Where(aT => aT.IsClass && !aT.IsAbstract && aT.IsAssignableTo(type)))
            {
                var section = configuration.GetSection(sectionType.Name).Get(sectionType)!;

                services.AddSingleton(sectionType, section);
            }

            return services;
        }

        public static IServiceCollection RegisterGenericTypes(this IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                var assembly = type.Assembly;

                foreach (var assemblyType in assembly.GetTypes().Where(aT => aT.IsClass && !aT.IsAbstract))
                {
                    foreach (var i in assemblyType.GetInterfaces())
                    {
                        if (i.IsGenericType && i.GetGenericTypeDefinition() == type)
                        {
                            var interfaceType = type.MakeGenericType(i.GetGenericArguments());

                            services.AddTransient(interfaceType, assemblyType);
                        }
                    }
                }
            }

            return services;
        }

        public static IServiceCollection RegisterMarkerTypes(this IServiceCollection services, params Type[] types)
        {
            foreach (var type in types)
            {
                var assembly = type.Assembly;

                foreach (var assemblyType in assembly.GetTypes().Where(aT => aT.IsClass && !aT.IsAbstract))
                {
                    if (type.IsAssignableFrom(assemblyType))
                    {
                        foreach (var i in assemblyType.GetInterfaces())
                        {
                            if (i != type && type.IsAssignableFrom(i))
                            {
                                services.AddTransient(i, assemblyType);
                            }
                        }
                    }
                }
            }

            return services;
        }
    }
}
