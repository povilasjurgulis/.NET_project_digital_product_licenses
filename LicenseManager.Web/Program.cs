using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

// Konfigūruojame naudotojų registraciją, prisijungimą ir roles: 
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // Leidžia prisijungti nepatvirtinus paskyros su email
        options.User.RequireUniqueEmail = true; // Kiekvienas naudotojas turi turėti unikalų email
        options.Password.RequiredLength = 8; // Minimalus slaptažodžio ilgis - 8 simboliai
    })
    .AddRoles<IdentityRole>() // Prideda rolių valdymą
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseRequestLocalization("lt-LT"); // Nustato lietuvišką skaičių formatą


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // Nustato, kas yra prisijungęs
app.UseAuthorization(); // Tikrina, kokius veiksmus gali atlikti

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Užregistruojami Razor Pages maršrutai (pvz. prisijungimui ir registracijai):
app.MapRazorPages();

// Šis kodas leidžia per terminalo argumentą --assign-admin priskirti administratoriaus teises:
if (args.Contains("--assign-admin")) // Patikrinamas paleidimo argumentas
{
    if (!app.Environment.IsDevelopment()) // Patikrinama programos aplinka
    {
        throw new InvalidOperationException(
            "Ši komanda skirta Development aplinkai.");
    }

    string? adminEmail = app.Configuration["AdminSetup:Email"]; // Gaunamas admin email

    if (string.IsNullOrWhiteSpace(adminEmail)) // Patikrinama, ar email nurodytas
    {
        throw new InvalidOperationException(
            "Nenurodytas AdminSetup:Email nustatymas.");
    }

    await AdminSetup.AssignAsync(app.Services, adminEmail); // Priskiriame admin rolė

    Console.WriteLine("Administratoriaus teisės priskirtos.");

    return;
}

app.Run();
