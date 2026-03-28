using Library.ItemAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Lägger till controllers + Swagger för testning
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Lägger till CORS så att React-appen lokalt får anropa API:t
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Databas (SQLite)
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

var app = builder.Build();

// Hämtar API-nyckeln från konfiguration, annars används fallback lokalt
var apiKey = builder.Configuration["ApiKey"] ?? "test123";

app.UseSwagger();
app.UseSwaggerUI();

// Enkel test-route
app.MapGet("/", () => "ItemAPI is running");

app.UseHttpsRedirection();

// Tillåter React-appen att prata med API:t lokalt
app.UseCors("AllowReactApp");

// Middleware som skyddar alla /api endpoints
app.Use(async (context, next) =>
{
    // Gäller bara API-anrop (t.ex. /api/Items)
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        // Försöker läsa headern "X-API-Key"
        if (!context.Request.Headers.TryGetValue("X-API-Key", out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key missing.");
            return;
        }

        // Jämför med vår riktiga nyckel
        if (apiKey != extractedApiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API key.");
            return;
        }
    }

    // Släpper igenom requesten om allt är OK
    await next();
});

app.UseAuthorization();

app.MapControllers();

// Skapar databasen om den inte finns
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    db.Database.EnsureCreated();
}

app.Run();