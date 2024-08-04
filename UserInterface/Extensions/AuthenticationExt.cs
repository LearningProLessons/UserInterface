using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

                                        
namespace UserInterface.Extensions;
public static class AuthenticationExt
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
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure this is set to Always in production
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

            foreach (var scope in authSettings.Scopes)
            {
                options.Scope.Add(scope);
            }
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };

            // Set the URI for sign-out
            options.SignedOutCallbackPath = authSettings.SignOutCallbackPath;
            options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = authSettings.Authority;
            options.Audience = authSettings.JwtBearer.Audience;
            options.RequireHttpsMetadata = authSettings.JwtBearer.RequireHttpsMetadata;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authSettings.Authority,
                ValidAudience = authSettings.JwtBearer.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.JwtBearer.IssuerSigningKey!)) // Use the secret key
            };
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {

            // Define policies based on scopes
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireClaim("scope", "all.read", "all.write"));

            options.AddPolicy("UserPolicy", policy =>
                policy.RequireClaim("scope", "order.read", "order.write"));
        });

        return services;
    }

}
