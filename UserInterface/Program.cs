using UserInterface.Extensions;
using UserInterface.Services;
using UserInterface.Services.Reasons;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register IHttpClientFactory
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IReasonService, ReasonService>();
builder.Services.AddCustomAuthentication();
builder.Services.AddCustomAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCustomPipeline();

app.Run();
