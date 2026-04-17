using Agendado.Model;

namespace Agendado.Extensions.Email
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
