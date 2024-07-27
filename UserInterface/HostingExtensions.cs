using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using UserInterface;



public static class HostingExtensions
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
        .AddCookie()
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
