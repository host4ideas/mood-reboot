using Azure.Storage.Blobs;
using Ganss.Xss;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Helpers;
using MoodReboot.Hubs;
using MoodReboot.Services;
using MvcLogicApps.Services;
using NugetMoodReboot.Helpers;

var builder = WebApplication.CreateBuilder(args);

// IHttpClientFactory
builder.Services.AddHttpClient();

// HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// HtmlSanitizer
builder.Services.AddSingleton<HtmlSanitizer>();

// SignalR
string signalrCnn = builder.Configuration.GetConnectionString("SignalR");
builder.Services.AddSignalR().AddAzureSignalR(signalrCnn);

// Azure storage blobs
string azureKeys = builder.Configuration.GetValue<string>("AzureKeys:StorageAccount");
BlobServiceClient blobServiceClient = new(azureKeys);
builder.Services.AddSingleton(blobServiceClient);
builder.Services.AddTransient<ServiceStorageBlob>();

// Api Services
builder.Services.AddTransient<ServiceApiCenters>();
builder.Services.AddTransient<ServiceApiContents>();
builder.Services.AddTransient<ServiceApiContentGroups>();
builder.Services.AddTransient<ServiceApiCourses>();
builder.Services.AddTransient<ServiceApiUsers>();

// Logic apps
builder.Services.AddTransient<ServiceLogicApps>();

// Content moderator
builder.Services.AddTransient<ServiceContentModerator>();

// Helpers
builder.Services.AddSingleton<HelperApi>();
builder.Services.AddSingleton<HelperCryptography>();
builder.Services.AddSingleton<HelperFileAzure>();
builder.Services.AddSingleton<HelperMail>();

// Session
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

// BBDD
string connectionString = builder.Configuration.GetConnectionString("SqlMoodReboot");

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

app.MapHub<ChatHub>("/chatHub");

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}"
        );
});

app.Run();
