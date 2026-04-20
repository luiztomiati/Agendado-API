using Agendado.Interface.Repository;
using Agendado.Repository;

namespace Agendado.Extensions.DependencyInjection
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IServicoRepository, ServicoRepository>();
            services.AddScoped<IServicoUsuarioRepository, ServicoUsuarioRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            return services;
        }
    }
}
