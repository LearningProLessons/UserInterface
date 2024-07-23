using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
 


public static class HostingExtensions
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authConfig = configuration.GetSection("Authentication");
        var authority = authConfig.GetValue<string>("Authority");
        var clientId = authConfig.GetValue<string>("ClientId");
        var clientSecret = authConfig.GetValue<string>("ClientSecret");
        var responseType = authConfig.GetValue<string>("ResponseType");
        var signOutCallbackPath = authConfig.GetValue<string>("SignOutCallbackPath");

        var jwtBearerConfig = authConfig.GetSection("JwtBearer");
        var audience = jwtBearerConfig.GetValue<string>("Audience");
        var requireHttpsMetadata = jwtBearerConfig.GetValue<bool>("RequireHttpsMetadata");
        var issuerSigningKey = jwtBearerConfig.GetValue<string>("IssuerSigningKey");

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
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = authority;
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.ResponseType = responseType!;
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;

            foreach (var scope in authConfig.GetSection("Scopes").Get<List<string>>()!)
            {
                options.Scope.Add(scope);
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };

            // Set the URI for sign-out
            options.SignedOutCallbackPath = signOutCallbackPath;
            options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = authority;
            options.Audience = audience;
            options.RequireHttpsMetadata = requireHttpsMetadata;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authority,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(issuerSigningKey!)) // Use the secret key
            };
        });

        return services;
    }
    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy =>
                policy.RequireRole("admin"));
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }

    
    public static IApplicationBuilder UseCustomPipeline(this IApplicationBuilder app)
    {
        // No direct access to Environment here
        // Moving environment check to Program.cs or Startup.cs

        if (app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        else
        {
            // In production
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });

        return app;
    }
}
