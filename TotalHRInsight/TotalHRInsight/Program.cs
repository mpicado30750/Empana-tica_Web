using Microsoft.EntityFrameworkCore;
using TotalHRInsight.DAL;
using Microsoft.AspNetCore.Identity;
using TotalHRInsight.Hubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConnTHRIDB") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

// Configurar los contextos de base de datos
builder.Services.AddDbContext<TotalHRInsightDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configurar los servicios de identidad
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Agregar SignalR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// Mapear rutas y hubs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Mapear el NotificationHub
app.MapHub<NotificationHub>("/notificationHub");

// CalcularPlanilla
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "calcularPlanilla",
        pattern: "Planillas/CalcularPlanilla",
        defaults: new { controller = "Planillas", action = "CalcularPlanilla" });
});

app.Run();
