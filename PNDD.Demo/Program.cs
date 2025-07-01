// Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PNDD.Demo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));


var app = builder.Build();

// Enable routing & MVC
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transaction}/{action=Create}/{id?}");

app.Run();

// Required for test project
public partial class Program { }
