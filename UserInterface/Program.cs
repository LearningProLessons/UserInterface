var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add custom authentication and authorization services
builder.Services.AddCustomAuthentication();
builder.Services.AddCustomAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCustomPipeline();

app.Run();
