using Microsoft.AspNetCore.Mvc;

// ProductsController apdoroja užklausas, susijusias su produktų puslapiu
public class ProductsController : Controller // ProductsController paveldi funkcionalumą iš ASP.NET Core Controller klasės
{
    public IActionResult Index() // IActionResult - metodas, kuris grąžina ASP.NET Core veiksmo rezultatą
    {
        List<Product> products = new List<Product>
        {
            new Product(1, "Shake Presets", 47m),
            new Product(2, "Viral Transitions", 57m),
            new Product(3, "Impact Presets", 57m)
        };
        
        return View(products); // Paruošia rezultatą, kuriame nurodomas atvaizduotinas View ir jam perduoda products sąrašą
    }
}