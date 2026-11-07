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

    // Atidarome formą:
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateProductViewModel());
    }

    // Priimame formos duomenis:
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        if (!ModelState.IsValid || model.Price is null)
        {
            return View(model);
        }

        // Iš įvestų duomenų sukuriamas Product objektas
        Product product = new Product(
            model.Name.Trim(),
            model.Price.Value);

        _context.Products.Add(product); // Paruošiamas įrašymas
        await _context.SaveChangesAsync(); // Išsaugomas produktas SQLite duomenų bazėje

        return RedirectToAction(nameof(Index)); // Naudotojas nukreipiamas į produktų sąrašą
    }
}