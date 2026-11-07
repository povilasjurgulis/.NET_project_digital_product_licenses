using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

public class CreateProductViewModel
{
    [Display(Name = "Pavadinimas")]
    [Required(ErrorMessage = "Įveskite produkto pavadinimą.")]
    [StringLength(100, ErrorMessage = "Pavadinimas negali viršyti 100 simbolių.")]
    public string Name {get; set;} = string.Empty;

    [Display(Name ="Kaina, EUR")]
    [Required(ErrorMessage = "Įveskite produkto kainą.")]
    [Range(0, double.MaxValue, ErrorMessage = "Kaina negali būti neigiama.")]
    public decimal? Price {get; set;}
}