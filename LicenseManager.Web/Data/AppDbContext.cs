using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // Visa reikalinga šio konstruktoriaus funkcija yra perduoti nustatymus tėvinei klasei, todėl čia nėra jokio kodo.
    }

    // Products suteikia prieigą prie produktų įrašų (čia Products yra AppDbContext property):
    public DbSet<Product> Products => Set<Product>(); // Leidžia dirbti su duomenų bazėje saugomais produktais

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product(1, "Shake Presets", 47m),
            new Product(2, "Viral Transitions", 57m),
            new Product(3, "Impact Presets", 57m)
        );
    }
}