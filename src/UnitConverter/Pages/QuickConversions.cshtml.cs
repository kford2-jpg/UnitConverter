using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnitConverter.Models;
using UnitConverter.Services;

namespace UnitConverter.Pages;

public class QuickConversionsModel : PageModel
{
    private readonly IConversionService _conversionService;
    private readonly ILogger<QuickConversionsModel> _logger;

    public QuickConversionsModel(
        IConversionService conversionService,
        ILogger<QuickConversionsModel> logger)
    {
        _conversionService = conversionService;
        _logger = logger;
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
        _logger.LogInformation(
            "Received conversion request for {ConversionType} with input {Input}",
            conversionType,
            input);

        if (!decimal.TryParse(input, out decimal value))
        {
            _logger.LogWarning(
                "Invalid input {Input} for conversion {ConversionType}",
                input,
                conversionType);

            ErrorMessage = "Input must be a valid number.";
            return Page();
        }

        try
        {
            decimal result = _conversionService.Convert(value, conversionType);
            Output = result.ToString();

            _logger.LogInformation(
                "Converted {Input} using {ConversionType} with result {Result}",
                value,
                conversionType,
                result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Unknown conversion type {ConversionType} for input {Input}",
                value,
                conversionType);

            ErrorMessage = "Unknown conversion type.";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Conversion failed for input {Input} using {ConversionType}",
                value,
                conversionType);

            ErrorMessage = "An unexpected error occurred during conversion.";
            return Page();
        }

        return Page();
    }
}
