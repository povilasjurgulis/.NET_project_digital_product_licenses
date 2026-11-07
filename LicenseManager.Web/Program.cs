using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Sukuriame kelią iki duomenų bazės failo (SQLite duomenų bazės failas licensemanager.db).
string databasePath = Path.Combine(
    builder.Environment.ContentRootPath,
    "licenseManager.db"
);

// Užregistruojamas AppDbContext, kad jį būtų galima naudoti kitose klasėse (pvz., Controller).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}")); // Nurodoma, kad bus naudojama SQLite DB.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
