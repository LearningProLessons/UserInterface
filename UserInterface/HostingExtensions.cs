using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;



public static class HostingExtensions
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
      .AddCookie()
      .AddOpenIdConnect(options =>
      {
          options.Authority = "https://localhost:5001"; // Identity Server URL
          options.ClientId = "SappPlusCompanyUIClient";
          options.ClientSecret = "K8T1L7J9V0D3R+4W6Fz5X2Q8B1N7P3C4G0A9J7R8H6=";
          options.ResponseType = "code";
          options.SaveTokens = true;
          options.Scope.Add("openid");
          options.Scope.Add("profile");
          options.Scope.Add("scope_sapplus");
          options.GetClaimsFromUserInfoEndpoint = true;
          options.Events = new OpenIdConnectEvents
          {
              OnRedirectToIdentityProvider = context =>
              {
                  var requestedScopes = context.ProtocolMessage.Scope;
                  Console.WriteLine($"Requested Scopes: {requestedScopes}");
                  return Task.CompletedTask;
              }
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
