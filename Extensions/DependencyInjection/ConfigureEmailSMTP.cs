using Agendado.Infraestructure.Settings;

namespace Agendado.Extensions.DependencyInjection
{
    public static class ConfigureEmailSMTP
    {
        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));

            return services;
        }
    }
}
