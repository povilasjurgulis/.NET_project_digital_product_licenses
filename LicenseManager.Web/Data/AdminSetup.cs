// Ši klasė nesukuria naujos paskyros. Ji suteikia admin teises jau egzistuojančiai paskyrai.

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class AdminSetup
{
    // Metodas, priskiriantis naudotojui Admin rolę:
    public static async Task AssignAsync(IServiceProvider services, string email)
    {
        using IServiceScope scope = services.CreateScope();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>(); // Gauna objektą, skirtą naudotojų valdymui

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>(); // Gauna objektą, skirtą rolių valdymui

        IdentityUser? user = await userManager.FindByEmailAsync(email.Trim());

        if (user is null) // Patikrina, ar naudotojas buvo rastas
        {
            throw new InvalidOperationException(
                "Paskyra nerasta. Patikrinkite nurodytą el. paštą.");
        }

        if (!await roleManager.RoleExistsAsync("Admin")) // Jei Admin rolė neegzistuoja, ji sukuriama
        {
            IdentityResult createResult = await roleManager.CreateAsync(new IdentityRole("Admin")); // Sukuria Admin rolę, jei jos nėra

            CheckResult(createResult);
        }

        if (!await userManager.IsInRoleAsync(user, "Admin")) // Jei naudotojas dar nėra administratorius, priskiriama Admin rolė
        {
            IdentityResult assignResult = await userManager.AddToRoleAsync(user, "Admin"); // Priskiria naudotojui Admin rolę

            CheckResult(assignResult);
        }
    }

    private static void CheckResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            string errors = string.Join(
                "; ",
                result.Errors.Select(error => error.Description));

            throw new InvalidOperationException(errors);
        }
    }
}