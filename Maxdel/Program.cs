using Maxdel.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Maxdel.Repositorio;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
    });

builder.Services.AddDbContext<DbEntities>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Pizzeria"))
);

builder.Services.AddTransient<IAuthRepositorio, AuthRepositorio>();
builder.Services.AddTransient<IHomeAdminRepositorio, HomeAdminRepositorio>();
builder.Services.AddTransient<IHomeRepositorio, HomeRepositorio>();
builder.Services.AddTransient<IMenuRepositorio, MenuRepositorio>();
builder.Services.AddTransient<IPedidosRepositorio, PedidosRepositorio>();
builder.Services.AddTransient<IPersonaRepositorio, PersonaRepositorio>();
builder.Services.AddTransient<IProcesarCompraRepositorio, ProcesarCompraRepositorio>();
builder.Services.AddTransient<IProductosRepositorios, ProductosRepositorios>();
builder.Services.AddTransient<IRegisterAdminRepositorio, RegisterAdminRepositorio>();
builder.Services.AddTransient<ITrackingRepositorio, TrackingRepositorio>();
builder.Services.AddTransient<IVenderRepositorio, VenderRepositorio>();
builder.Services.AddTransient<IVentasRepositorio, VentasRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
