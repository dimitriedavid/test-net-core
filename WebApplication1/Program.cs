using Microsoft.EntityFrameworkCore;
using WebApplication1.Framework.Db;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration["ConnectionStrings"];

// Add services to the container.
var mvcBuilder = builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString, b => b.MigrationsAssembly("WebApplication1")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    DbInitializer.Initialize(context);
}

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
