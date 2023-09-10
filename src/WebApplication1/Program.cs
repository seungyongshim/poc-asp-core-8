using System.Diagnostics;
using WebApplication1.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();
app.MapHealthChecks("/health");

app.MapGet("/", (HttpContext ctx) => TypedResults.Ok(new
{
    Text = "Hello"
}))
.AddEndpointFilter<ResponseAddTraceIdFilter>()
.WithOpenApi();

app.Run();
