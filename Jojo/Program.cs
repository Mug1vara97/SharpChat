using Jojo.Models;
using Microsoft.EntityFrameworkCore;
using SignalRApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql("Server=localhost; Database=Chat; User Id=postgres; Password=1000-7;"));

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.MapHub<ChatHub>("/chat");
//app.MapHub<ChatGroupHub>("/chatgroup");

app.Run();
