using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// Klasės paskirtis – nuskaityti produktus iš SQLite duomenų bazės, surikiuoti juos pagal ID ir perduoti internetiniam puslapiui (View).
public class ProductsController : Controller
{
    private readonly AppDbContext _context; // klasės laukas, kur saugoma nuoroda į mano db kontekstą.

    // Per Dependency Injection gaunamas AppDbContext:
    public ProductsController(AppDbContext context) // ASP.NET Core pateikia kontekstą valdikliui.
    {
        _context = context;
    }

     // Asinchroniškai gaunami produktai iš duomenų bazės:
    public async Task<IActionResult> Index()
    {
        List<Product> products = await _context.Products
            .AsNoTracking() // Nesekami objektų pakeitimai
            .OrderBy(product => product.Id) // Rikiavimas pagal ID didėjimo tvarka
            .ToListAsync(); // Užklausa įvykdoma, rezultatai sudedami į sąrašą

        return View(products); // Produktų sąrašas perduodamas į View.
    }
}