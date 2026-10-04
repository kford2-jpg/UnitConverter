using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnitConverter.Models;
using UnitConverter.Services;

namespace UnitConverter.Pages;

public class QuickConversionsModel : PageModel
{
    private readonly IConversionService _conversionService;

    public QuickConversionsModel(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    public IEnumerable<SelectListItem> PoundOptions =>
    [
        new("1 pound", "1"),
        new("5 pounds", "5"),
        new("10 pounds", "10"),
        new("25 pounds", "25"),
        new("50 pounds", "50")
    ];

    public string Output { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public IActionResult OnGetMilesToKilometers(string input)
    {
        return PerformConversion(input, ConversionTypes.MilesToKilometers);
    }

    public IActionResult OnGetKilometersToMiles(string input)
    {
        return PerformConversion(input, ConversionTypes.KilometersToMiles);
    }

    public IActionResult OnGetFahrenheitToCelsius(string input)
    {
        return PerformConversion(input, ConversionTypes.FahrenheitToCelsius);
    }

    public IActionResult OnGetCelsiusToFahrenheit(string input)
    {
        return PerformConversion(input, ConversionTypes.CelsiusToFahrenheit);
    }

    public IActionResult OnGetPoundsToKilograms(string input)
    {
        return PerformConversion(input, ConversionTypes.PoundsToKilograms);
    }

    public IActionResult OnGetKilogramsToPounds(string input)
    {
        return PerformConversion(input, ConversionTypes.KilogramsToPounds);
    }

    public IActionResult OnGetMilesToFeet(string input)
    {
        return PerformConversion(input, ConversionTypes.MilesToFeet);
    }

    public IActionResult OnGetFeetToMiles(string input)
    {
        return PerformConversion(input, ConversionTypes.FeetToMiles);
    }

    private IActionResult PerformConversion(
        string input,
        string conversionType)
    {
        if (!decimal.TryParse(input, out decimal value))
        {
            ErrorMessage = "Input must be a valid number.";
            return Page();
        }

        try
        {
            Output = _conversionService.Convert(value, conversionType).ToString();
        }
        catch (InvalidOperationException)
        {
            ErrorMessage = "Unknown conversion type.";
            return Page();
        }

        return Page();
    }
}
