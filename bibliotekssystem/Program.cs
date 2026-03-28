using bibliotekssystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Lägg till HttpClient till LoanService
builder.Services.AddHttpClient<LoanService>((serviceProvider, httpClient) =>
{
    // Hämta config
    var config = serviceProvider.GetRequiredService<IConfiguration>();

    // Hämta adress till LoanService ifrån config
    string adress = config.GetValue<string>("LoanServiceAdress") ?? "";
    
    // Api nyckel
    string apiKey = config["Authentication:ApiKey"] ?? ""; 
    
    httpClient.BaseAddress = new Uri(adress);
    
    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
    }
});

// Lägg till HttpClient till ItemService
builder.Services.AddHttpClient<ItemService>((serviceProvider, httpClient) =>
{
    // Hämta config
    var config = serviceProvider.GetRequiredService<IConfiguration>();

    // Hämta adress till ItemService ifrån config
    string adress = config.GetValue<string>("ItemServiceAddress") ?? "";

    httpClient.BaseAddress = new Uri(adress);
});

// Lägg till HttpClient till UserService
builder.Services.AddHttpClient<AccountService>((serviceProvider, httpClient) =>
{
    // Hämta config
    var config = serviceProvider.GetRequiredService<IConfiguration>();

    // Hämta adress
    string adress = config.GetValue<string>("UserServiceAdress") ?? "";

    httpClient.BaseAddress = new Uri(adress);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/Index");

builder.Services.AddScoped<ReminderService>();

builder.Services.AddHttpClient("ReminderService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ReminderServiceAddress"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();