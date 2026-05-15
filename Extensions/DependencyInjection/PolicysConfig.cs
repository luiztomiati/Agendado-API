namespace Agendado.Extensions.DependencyInjection
{
    public static class PolicysConfig
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnboardingConcluido", policy =>
                    policy.RequireClaim("PrimeiroLogin", "false"));
                options.AddPolicy("EmailConfirmado", policy =>
                    policy.RequireClaim("EmailConfirmed", "true"));
            });

            return services;
        }
    }
}
