using Agendado.Interface.Service;
using Agendado.Service;

namespace Agendado.Extensions.Service
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<IServicoService, ServicoService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<TokenService>();

            return services;
        }
    }
}
