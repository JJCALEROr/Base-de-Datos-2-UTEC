using InventarioVentasMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Servicios MVC + Runtime Compilation
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme
    )
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";

        options.AccessDeniedPath =
            "/Account/Login";
    });

// DbContext
builder.Services.AddDbContext<InventarioContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("InventarioContext")
    )
);

var app = builder.Build();

// Pipeline HTTP
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
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Configuración Rotativa
Rotativa.AspNetCore.RotativaConfiguration.Setup(
    app.Environment.WebRootPath,
    "../Rotativa"
);

app.Run();