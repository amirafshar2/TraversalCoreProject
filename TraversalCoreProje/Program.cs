using BusinessLayer.Container;
using DataAccessLayer.Concrate;
using EntityLayer.Concrate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using TraversalCoreProje.Infrastructure;

// Einheitliches Zahlen-/Datumsformat beim Model-Binding (z. B. Preis "1650.5" aus <input type="number">).
// Die Anzeige auf Deutsch erfolgt gezielt über ViewHelpers (de-DE).
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

// ---------- Datenbank (SQLite, liegt im Projekt unter App_Data) ----------
var connectionString = SqlitePath.Resolve(
    builder.Configuration.GetConnectionString("DefaultConnection") ?? Context.DefaultConnection,
    builder.Environment.ContentRootPath);
builder.Services.AddDbContext<Context>(options => options.UseSqlite(connectionString));

// ---------- Identity ----------
builder.Services.AddIdentity<User, Roll>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<Context>()
.AddDefaultTokenProviders()
.AddErrorDescriber<GermanIdentityErrorDescriber>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.LogoutPath = "/User/Logout";
    options.AccessDeniedPath = "/User/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// ---------- Business Layer + Data Access Layer (DI) ----------
builder.Services.ContainerDependencies();

// ---------- Demo-Modus ----------
builder.Services.Configure<DemoOptions>(builder.Configuration.GetSection("Demo"));
builder.Services.AddScoped<DatabaseInitializer>();
builder.Services.AddHostedService<DemoResetService>();

builder.Services.AddHttpClient();

// Standardmäßig muss jeder Controller angemeldet sein – öffentliche Seiten sind mit [AllowAnonymous] markiert.
builder.Services.AddControllersWithViews(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

// Hinter einem Reverse Proxy (z. B. Render/Docker) die Original-Protokolle übernehmen
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Datenbank anlegen/migrieren und beim ersten Start mit Demo-Daten füllen
using (var scope = app.Services.CreateScope())
{
    var demo = builder.Configuration.GetSection("Demo").Get<DemoOptions>() ?? new DemoOptions();
    await scope.ServiceProvider.GetRequiredService<DatabaseInitializer>()
        .InitializeAsync(reset: demo.Enabled && demo.ResetOnStartup);
}

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashbord}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
