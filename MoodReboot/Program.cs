using MoodReboot.Hubs;
using MoodReboot.Interfaces;
using MoodReboot.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddTransient<IStreamFileUploadService, StreamFileUploadLocalService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}"
    );

app.MapHub<ChatHub>("/chatHub");

app.Run();
