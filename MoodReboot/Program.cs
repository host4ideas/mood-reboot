using Ganss.Xss;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Hubs;
using MoodReboot.Services;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddTransient<IRepositoryCenters, ServiceApiCenters>();
builder.Services.AddTransient<IRepositoryContent, ServiceApiContents>();
builder.Services.AddTransient<IRepositoryContentGroups, ServiceApiContentGroups>();
builder.Services.AddTransient<IRepositoryCourses, ServiceApiCourses>();
builder.Services.AddTransient<IRepositoryUsers, ServiceApiUsers>();

builder.Services.AddSingleton<HelperApi>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromMinutes(120);
});

// Seguridad
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN_ONLY", policy => policy.RequireRole("ADMIN"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(
        CookieAuthenticationDefaults.AuthenticationScheme,
        config =>
        {
            config.AccessDeniedPath = "/Managed/AccessError";
        }
);

// Sessions
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// SignalR
builder.Services.AddSignalR();

string connectionString = builder.Configuration.GetConnectionString("SqlMoodReboot");

// Custom
builder.Services.AddSingleton<HtmlSanitizer>();

// Indicamos que queremos utilizar nuestras propias rutas
builder.Services.AddControllersWithViews(
    options => options.EnableEndpointRouting = false
).AddSessionStateTempDataProvider();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Managed/AccessError";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
});

app.MapHub<ChatHub>("/chatHub");

app.Run();
