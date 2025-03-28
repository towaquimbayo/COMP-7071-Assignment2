using Microsoft.Extensions.DependencyInjection;

namespace EmailNotifier
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailNotifier(this IServiceCollection services, EmailSettings settings)
        {
            services.AddSingleton(settings);
            services.AddSingleton<IEmailNotifier, EmailNotifierService>();
            return services;
        }
    }
}
