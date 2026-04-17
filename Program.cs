using Agendado.Data;
using Agendado.Extensions.Email;
using Agendado.Extensions.Repository;
using Agendado.Extensions.Service;
using Agendado.Extensions.Swagger;
using Agendado.Infraestructure;
using Agendado.Model;
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

builder.Services.AddRepositories();
builder.Services.AddServices();

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

builder.Services.AddEmailConfiguration(builder.Configuration);

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
