using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitOf;

namespace UnitConverter.Pages;

public class ConversionsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Input { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string ConversionType { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;

    public string DisplayConversionType { get; set; } = string.Empty;

    public void OnGet()
    {
        // Backwards compatibility with previous lessons.
        if (string.IsNullOrEmpty(ConversionType))
        {
            ConversionType = "Miles to Kilometers";
        }

        if (string.IsNullOrEmpty(Input))
        {
            Input = "3.1415";
        }

        ViewData["ConversionType"] = "Miles to Kilometers";
        ViewData["Title"] = "Conversions";

        double inputValue;

        try
        {
            inputValue = Convert.ToDouble(Input);
        }
        catch (FormatException)
        {
            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }
        catch (OverflowException)
        {
            ViewData["ErrorMessage"] = "Input is outside the valid numeric range.";
            return;
        }

        DisplayConversionType = ConversionType switch
        {
            "MilesToKilometers" => "Miles to Kilometers",
            "KilometersToMiles" => "Kilometers to Miles",
            "FahrenheitToCelsius" => "Fahrenheit to Celsius",
            "CelsiusToFahrenheit" => "Celsius to Fahrenheit",
            "PoundsToKilograms" => "Pounds to Kilograms",
            "KilogramsToPounds" => "Kilograms to Pounds",
            "MilesToFeet" => "Miles to Feet",
            "FeetToMiles" => "Feet to Miles",
            "Miles to Kilometers" => "Miles to Kilometers",
            _ => ConversionType
        };

        double result;

        try
        {
            result = ConversionType switch
            {
                "MilesToKilometers" =>
                    new Length().FromMiles(inputValue).ToKilometers(),

                "KilometersToMiles" =>
                    new Length().FromKilometers(inputValue).ToMiles(),

                "FahrenheitToCelsius" =>
                    new Temperature().FromFahrenheit(inputValue).ToCelsius(),

                "CelsiusToFahrenheit" =>
                    new Temperature().FromCelsius(inputValue).ToFahrenheit(),

                "PoundsToKilograms" =>
                    new Mass().FromPounds(inputValue).ToKilograms(),

                "KilogramsToPounds" =>
                    new Mass().FromKilograms(inputValue).ToPounds(),

                "MilesToFeet" =>
                    new Length().FromMiles(inputValue).ToFeet(),

                "FeetToMiles" =>
                    new Length().FromFeet(inputValue).ToMiles(),

                // Backwards compatibility
                "Miles to Kilometers" =>
                    new Length().FromMiles(inputValue).ToKilometers(),

                _ => throw new InvalidOperationException()
            };
        }
        catch (InvalidOperationException)
        {
            ViewData["ErrorMessage"] = "Unknown conversion type.";
            return;
        }

        Output = result.ToString();
    }
}
