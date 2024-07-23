using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    // Example settings
    options.Cookie.HttpOnly = false;
   // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure this is set correctly

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Redirect("/AccessDenied");
        return Task.CompletedTask;
    };
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://localhost:5001"; // Your IdentityServer URL
    options.ClientId = "Sample.UI";
    options.ClientSecret = "K8T1L7J9V0D3R+4W6Fz5X2Q8B1N7P3C4G0A9J7R8H6="; // Replace with actual secret
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("scope1");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };

    // Set the URI for sign-out
    options.SignedOutRedirectUri = "https://localhost:7076/signout-callback-oidc";

})
.AddJwtBearer(options =>
{
    options.Authority = "https://localhost:5001"; // Your IdentityServer URL
    options.Audience = "api1"; // The API resource you want to access
    options.RequireHttpsMetadata = false; // Set to true in production

    var key = Encoding.ASCII.GetBytes("ThisIsASecretKeyForDevelopmentOnly!");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key) // Use the secret key
    };
});

// Configure authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("admin"));
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
