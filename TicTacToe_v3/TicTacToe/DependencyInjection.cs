using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicTacToe.DataSource.Models;
using TicTacToe.DataSource.Services;
using TicTacToe.Domain.Services;
using TicTacToe.Web.Models;
using TicTacToe.Web.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllersWithViews();
        services.AddDbContext<AppDbContext>(options =>
            { options.UseNpgsql(config.GetConnectionString("DefaultConnection")); },
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        services.AddScoped<IGameDbService, GameDbService>();
        services.AddScoped<IUserDbService, UserDbService>();

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IGameService, GameService>();
        
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }

    public static IServiceCollection AddJwtBearerAuth(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,           ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,         ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,         IssuerSigningKey = AuthOptions.GetSymmetricAccessSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero /* for testing */
                    };
                    options.Events = new JwtBearerEvents {
                    OnMessageReceived = context =>
                    {
                        var token = context.HttpContext.Request.Cookies["access_token"];

                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;
                        
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var returnUrl = context.Request.Path;

                        context.HandleResponse();
                        context.Response.Redirect("/game/update-access?returnUrl=" + returnUrl);

                        return Task.CompletedTask;
                    } };
                });

        return services;
    }
}