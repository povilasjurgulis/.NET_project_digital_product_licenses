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

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<License> Licenses => Set<License>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Klientas gali turėti daug užsakymų.
        modelBuilder.Entity<Order>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(order => order.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Užsakymas turi daug eilučių.
        // Kiekviena eilutė priklauso vienam užsakymui.
        modelBuilder.Entity<Order>()
            .HasMany(order => order.Items)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // EF Core turi naudoti tikrąjį privatų sąrašą.
        modelBuilder.Entity<Order>()
            .Navigation(order => order.Items)
            .HasField("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Produktas gali būti daugelyje užsakymo eilučių.
        modelBuilder.Entity<OrderItem>()
            .HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Vienai užsakymo eilutei gali priklausyti
        // daugiausia viena licencija.
        modelBuilder.Entity<License>()
            .HasOne(license => license.OrderItem)
            .WithOne()
            .HasForeignKey<License>(license => license.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Licencijoje jau turimas ProductId taip pat susiejamas su produktu.
        modelBuilder.Entity<License>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(license => license.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Licencijos raktas duomenų bazėje turi būti unikalus.
        modelBuilder.Entity<License>()
            .HasIndex(license => license.LicenseKey)
            .IsUnique();

        modelBuilder.Entity<Product>().HasData(
            new Product(1, "Shake Presets", 47m),
            new Product(2, "Viral Transitions", 57m),
            new Product(3, "Impact Presets", 57m)
        );
    }
}