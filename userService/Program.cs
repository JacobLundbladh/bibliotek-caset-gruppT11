using Microsoft.EntityFrameworkCore;
using userService.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserServiceDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
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



// Lägg till services
builder.Services.AddControllers();       // för API controllers
builder.Services.AddEndpointsApiExplorer(); // swagger osv om du vill
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// Swagger
app.UseSwagger();
app.UseSwaggerUI();




app.UseAuthorization();

app.MapControllers();


app.Run();

