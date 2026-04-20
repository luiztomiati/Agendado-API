using Agendado.Interface.Service;
using Agendado.Service;

namespace Agendado.Extensions.DependencyInjection
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
            services.AddScoped<IServicoUsuarioService, ServicoUsuarioService>();
            services.AddScoped<IClienteService, ClienteService>();

            return services;
        }
    }
}
