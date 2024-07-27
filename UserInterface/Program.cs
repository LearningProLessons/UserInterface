using UserInterface.Extensions;
using UserInterface.Services;


var builder = WebApplication.CreateBuilder(args);


// Program.cs
builder.Services.AddHttpClient();
builder.Services.AddScoped<ApiService>(); // Change to Scoped
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddRazorPages();
builder.Services.AddCustomAuthentication();
builder.Services.AddCustomAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCustomPipeline();

app.Run();
