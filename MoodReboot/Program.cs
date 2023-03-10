using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Helpers;
using MoodReboot.Hubs;
using MoodReboot.Interfaces;
using MoodReboot.Repositories;
using MvcCoreUtilidades.Helpers;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("SqlMoodReboot");

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
// DB Context
builder.Services.AddDbContext<MoodRebootContext>(options => options.UseSqlServer(connectionString));
// Repositories
builder.Services.AddTransient<IRepositoryCourses, RepositoryCoursesSql>();
builder.Services.AddTransient<IRepositoryCenters, RepositoryCentersSql>();
builder.Services.AddTransient<IRepositoryContent, RepositoryContentSql>();
builder.Services.AddTransient<IRepositoryUsers, RepositoryUsersSql>();
builder.Services.AddTransient<IRepositoryContentGroups, RepositoryCntGroupsSql>();
builder.Services.AddTransient<IRepositoryFile, RepositoryFileSql>();
// Helpers
builder.Services.AddSingleton<HelperPath>();
builder.Services.AddTransient<HelperFile>();
builder.Services.AddSingleton<HelperJsonSession>();
builder.Services.AddSingleton<HelperMail>();
// Sessions
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromMinutes(10);
});
// Custom
builder.Services.AddSingleton<HtmlSanitizer>();

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

app.MapHub<ChatHub>("/chatHub");

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
    );

app.Run();
