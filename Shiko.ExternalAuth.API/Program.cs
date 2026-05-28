using Shiko.ExternalAuth.API.Endpoints;
using Shiko.ExternalAuth.API.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddHttpClient("AuthApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
   
});

builder.Services.AddScoped<GoogleAuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.MapExternalAuthEndpoints();

app.Run();



