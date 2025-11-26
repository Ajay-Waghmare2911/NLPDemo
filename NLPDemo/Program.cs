using Microsoft.EntityFrameworkCore;
using NLPDemo.Database;
using NLPDemo.Models;

var builder = WebApplication.CreateBuilder(args);

#region Database
var mySqlConStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
               options.UseSqlServer(mySqlConStr));
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

LandQueryTrainer.Initialize(); // Load ML model if exists

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
