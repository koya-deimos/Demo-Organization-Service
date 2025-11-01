using System.Text;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using webapi;
using webapi.Persistence;

var builder = WebApplication.CreateBuilder(args);

//koya commented
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddHttpLogging(o => { });
builder.Services.AddPersistence(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration) // Read configuration from appsettings.json
        .Enrich.FromLogContext(); // Ensure enrichers are applied
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Home page");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Organization Service </h1>
    <p> Welcome to the organization Service </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

app.MapGet("api/v1/projects", (ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching projects");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Projects </h1>
    <p> No Projects </p>
    <p>The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

app.MapGet("api/v1/spaces", (ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching spaces");
    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Spaces </h1>
    <p> No Spaces </p>
    <p>The time now in UTC is {now.ToUniversalTime().ToString()} </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");


app.MapGet("api/v1/organizations", async (ILogger<Program> logger, ApplicationDbContext dbContext) =>
{
    logger.LogInformation("Fetching organizations");
    var now = DateTime.UtcNow;

    var organizations = await dbContext.Organizations.ToListAsync();
    var html = new StringBuilder();
    html.Append(@"
<html>
<head>
<link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
</head>
<body>
<h1>Organizations</h1>
");

    if (organizations.Any())
    {
        html.Append("<ul>");
        foreach (var org in organizations)
        {
            html.Append($"<li>{org.Name}</li>");
        }
        html.Append("</ul>");
    }
    else
    {
        html.Append("<p>No organizations found.</p>");
    }

    html.Append(@"
</body>
</html>
");

    return Results.Text(html.ToString(), "text/html");
});//.Produces(200, contentType: "text/html");

app.MapPost("api/v1/organizations", async (ILogger<Program> logger, OrganizationRequest request, ApplicationDbContext dbContext) =>
{
    logger.LogInformation("Creating organizations");
    var organization = new Organization()
    {
        Description = request.Description,
        Name = request.Name,
        Id = Guid.CreateVersion7(),
    };
    dbContext.Organizations.Add(organization);
    await dbContext.SaveChangesAsync();
    logger.LogInformation("Created organization");

    var now = DateTime.UtcNow;
    return Results.Text(@$"
    <html>
    <head>
    <link rel='stylesheet' href='https://cdn.simplecss.org/simple-v1.css'>
    </head>
    <body>
    <h1> Organizations </h1>
    <p> {organization} Organization created </p>
    </body>
    </html>
    ", "text/html");
});//.Produces(200, contentType: "text/html");

app.UseHttpLogging();
app.Services.MigrateTransactionsDb();

app.Run("http://0.0.0.0:8080");

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
