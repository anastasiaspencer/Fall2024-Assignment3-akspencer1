using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_akspencer1.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Azure.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = new Uri("https://akspencer1keyvault.vault.azure.net/");

builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

var dbPassword = builder.Configuration["DatabasePassword"];
if (string.IsNullOrEmpty(dbPassword))
{
    throw new InvalidOperationException("Database password could not be retrieved from Key Vault.");
}

// Add services to the container. directly grab the password and put it in here building the service
var connectionString = $"Server=tcp:fall2024-assignment3-akspencer1.database.windows.net,1433;Initial Catalog=fall2024-assignment3-akspencer1;Persist Security Info=False;User ID=akspencer1;Password={dbPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

