using Agendado.Interface.Repository;
using Agendado.Repository;

namespace Agendado.Extensions.Repository
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IServicoRepository, ServicoRepository>();

            return services;
        }
    }
}
