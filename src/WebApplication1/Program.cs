using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.MapHealthChecks("/health");

app.MapGet("/", () =>
{
    return new
    {
        TraceId = Activity.Current?.Id ?? ""
    };
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
