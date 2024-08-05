using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;



namespace UserInterface.Extensions;
public static class Authentication
{

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var authSettings = configuration.GetSection("Authentication").Get<AuthenticationSettings>()!;

        services.AddHttpClient();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.Redirect("/AccessDenied");
                return Task.CompletedTask;
            };
        })
        .AddOpenIdConnect(options =>
        {
            options.Authority = authSettings.Authority;
            options.ClientId = authSettings.ClientId;
            options.ClientSecret = authSettings.ClientSecret;
            options.ResponseType = authSettings.ResponseType;
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            // Add scopes from configuration
            foreach (var scope in authSettings.Scopes)
            {
                options.Scope.Add(scope);
            }

            options.UsePkce = true;

            // Map claims from the IDP to the authentication properties
            options.ClaimActions.MapJsonKey("role", "role"); // Map role claim
            options.ClaimActions.MapJsonKey("name", "name"); // Map name claim
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
            options.AddPolicy("RequireEmployeeRole", policy => policy.RequireRole("employee"));
            // Add other role policies as needed
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/SignIn"; // Redirect to login page if unauthenticated
            options.AccessDeniedPath = "/AccessDenied"; // Redirect to access denied page
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        
        // services.AddSingleton<IAuthorizationHandler, AdminRoleAndClaimHandler>(); // Register the handler


        return services;
    }

}
