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
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // Cookis logik
    .AddCookie(Options => Options.LoginPath = "/Account/Index");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStaticFiles();

app.Run();