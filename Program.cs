using Agendado.Data;
using Agendado.Infraestructure;
using Agendado.Interface.Repository;
using Agendado.Interface.Service;
using Agendado.Model;
using Agendado.Repository;
using Agendado.Service;
using Agendado.WebAPI.Extensions;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();

builder.Services.AddScoped<TokenService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityCore<AgendadoUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<AgendadoUser>>();

var key = Environment.GetEnvironmentVariable("JWT__KEY");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = Environment.GetEnvironmentVariable("JWT__AUDIENCE"),
            ValidIssuer = Environment.GetEnvironmentVariable("JWT__ISSUER"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!)),
        };
        opt.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
    });
builder.Services.ConfigureSwagger();


builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("reset-password", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(5);
        opt.PermitLimit = 3;
    });
});

var smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT");
int.TryParse(smtpPort, out int port);

builder.Services.AddSingleton(new EmailSettings
{
    SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? "",
    Port = port,
    Email = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? "",
    Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? ""
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeeder.SeedRoles(roleManager);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHsts();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
