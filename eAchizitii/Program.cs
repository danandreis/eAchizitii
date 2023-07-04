using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using eAchizitii.Data;
using eAchizitii.Data.Services;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNotyf(config => 
    { config.DurationInSeconds = 3; config.IsDismissable = false; 
        config.Position = NotyfPosition.BottomRight;});

builder.Services.AddDbContext<AppDbContext>(options=>options.
    UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));


//Configure Services
builder.Services.AddScoped<IProduseService, ProduseService>();
builder.Services.AddScoped<IAdreseLivrareService, AdreseLivrareService>();
builder.Services.AddScoped<IComenziService, ComenziService>();
builder.Services.AddScoped<IMesajeService, MesajeService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IInfoComandaService, InfoComandaService>();

//Autentificarea utilizatorilor
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

});


builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>()!.HttpContext!.User);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseNotyf();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Adauga administratorul
CreateAdminAccount.AddAdminAsync(app).Wait();

app.Run();