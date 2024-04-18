using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using StoriesOfTheLand.Controllers;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using Microsoft.Extensions.DependencyInjection;
using StoriesOfTheLand.Views.Shared.Components.Footer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/{0}.cshtml");
    });
builder.Services.AddDbContext<StoriesOfTheLandContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("StoriesOfTheLandContext") ?? throw new InvalidOperationException("Connection string 'StoriesOfTheLandContext' not found.")));

// Obtain the current status, if it's in the httpsSeedDataNoAuth environment. 
bool bypassAuthentication = bool.Parse(Environment.GetEnvironmentVariable("BYPASS_AUTHENTICATION") ?? "false");

// Normally, add authentication services like Camilo did
if (!bypassAuthentication)
{
    // Set up authentication only if not bypassed
    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
            options.SignedOutCallbackPath = "/signout-callback-oidc"; // Default callback path
        });
}

// OTHERWISE, it's in a dev environment for testing, so add the healper class
if (bypassAuthentication)
{
    // This adds the helper class that could really be placed anywhere that enables anonymous access to EVERY controller
    builder.Services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
}

builder.Services.AddDistributedMemoryCache();

// Configure cookie policy and settings
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MySessionCookie";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin() // Allow requests from any origin
            .AllowAnyMethod() // Allow any HTTP method
            .AllowAnyHeader(); // Allow any HTTP headers
    });
});


// Add services to the container.without a global authorization filter
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMicrosoftIdentityUI();


builder.Services.AddScoped<SponsorService>();
builder.Services.AddScoped<FooterViewComponent>();
builder.Services.AddScoped<IAnalyticsDataClient, RealAnalyticsDataClient>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    using (var context = new StoriesOfTheLandContext(
    services.GetRequiredService<
        DbContextOptions<StoriesOfTheLandContext>>()))
    {
        context.Database.EnsureCreated();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");

        }
        else
        {
            SeedData.Initialize(services, context);
        }
    }
}

app.UseStaticFiles();
app.UseCors();
app.UseRouting();
app.UseSession();
// VERY IMPORTANT - Authentication must be before authorization otherwise it will break
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();