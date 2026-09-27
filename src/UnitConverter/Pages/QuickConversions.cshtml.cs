using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UnitConverter.Pages;

public class QuickConversionsModel : PageModel
{
    public IEnumerable<SelectListItem> PoundOptions =>
    [
        new("1 pound", "1"),
        new("5 pounds", "5"),
        new("10 pounds", "10"),
        new("25 pounds", "25"),
        new("50 pounds", "50")
    ];

    public IActionResult OnGetMilesToKilometers(string input)
    {
        return RedirectToConversion(
            ConversionTypes.MilesToKilometers,
            input);
    }

    public IActionResult OnGetKilometersToMiles(string input)
    {
        return RedirectToConversion(
            ConversionTypes.KilometersToMiles,
            input);
    }

    public IActionResult OnGetFahrenheitToCelsius(string input)
    {
        return RedirectToConversion(
            ConversionTypes.FahrenheitToCelsius,
            input);
    }

    public IActionResult OnGetCelsiusToFahrenheit(string input)
    {
        return RedirectToConversion(
            ConversionTypes.CelsiusToFahrenheit,
            input);
    }

    public IActionResult OnGetPoundsToKilograms(string input)
    {
        return RedirectToConversion(
            ConversionTypes.PoundsToKilograms,
            input);
    }

    public IActionResult OnGetKilogramsToPounds(string input)
    {
        return RedirectToConversion(
            ConversionTypes.KilogramsToPounds,
            input);
    }

    public IActionResult OnGetMilesToFeet(string input)
    {
        return RedirectToConversion(
            ConversionTypes.MilesToFeet,
            input);
    }

    public IActionResult OnGetFeetToMiles(string input)
    {
        return RedirectToConversion(
            ConversionTypes.FeetToMiles,
            input);
    }

    private IActionResult RedirectToConversion(
        string conversionType,
        string input)
    {
        return RedirectToPage(
            "/Conversions",
            new
            {
                ConversionType = conversionType,
                Input = input
            });
    }
}
