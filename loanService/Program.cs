using loanService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new loanService.DateTimeConverter());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Loan Service API",
        Version = "v1"
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi(); -Negraderade till net 8 så detta funkar inte längre

// Konfigurera databas 
builder.Services.AddDbContext<LoanServiceDbContext>(options =>
{
 options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
} );

// För att fixa cors, så react kan kommuniera med loan api
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    }); 
});



var app = builder.Build();

// Migrera databas
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<LoanServiceDbContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
//    app.MapOpenApi();  -Negraderade till net 8 så detta funkar inte längre
    app.MapScalarApiReference();
}



// Swagger
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();