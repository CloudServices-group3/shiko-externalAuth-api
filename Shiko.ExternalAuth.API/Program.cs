using Shiko.ExternalAuth.API.Endpoints;
using Shiko.ExternalAuth.API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddHttpClient("AuthApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("X-Api-Key", builder.Configuration["AuthApi:ApiKey"]);
});

builder.Services.AddScoped<GoogleAuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapExternalAuthEndpoints();

// TEMP: mock av auth endpoint, tas bort sen
app.MapPost("/api/auth/external-login", (ExternalLoginRequest request) =>
{
    return Results.Ok(new
    {
        accessToken = "fake-access-token",
        refreshToken = "fake-refresh-token"
    });
});

app.Run();

