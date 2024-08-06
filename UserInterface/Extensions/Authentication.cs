using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserInterface.Configs;



namespace UserInterface.Extensions;
public static class AuthenticationExt
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var authSettings = configuration.Get<AppSettings>()!.Authentication;

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
            options.UsePkce = true;
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
            options.ClaimActions.MapJsonKey("role", "role"); // Map role claim
            options.ClaimActions.MapJsonKey("name", "name"); // Map name claim
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
            options.AddPolicy("AdminRolesPolicy", policy =>
                   policy.RequireRole("PegahAdmin", "MihanAdmin"));
        });

        return services;
    }

}


