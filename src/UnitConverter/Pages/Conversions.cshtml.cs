using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitConverter;
using UnitOf;

namespace UnitConverter.Pages;

public class ConversionsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public ConversionModel Conversion { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Input
    {
        get => Conversion.Input;
        set => Conversion.Input = value;
    }

    [BindProperty(SupportsGet = true)]
    public string ConversionType
    {
        get => Conversion.ConversionType;
        set => Conversion.ConversionType = value;
    }

    public string Output
    {
        get => Conversion.Output;
        set => Conversion.Output = value;
    }

    public string DisplayConversionType { get; set; } = string.Empty;

    public void OnGet()
    {
        if (string.IsNullOrEmpty(Conversion.ConversionType))
        {
            Conversion.ConversionType = ConversionTypes.MilesToKilometers;
        }

        if (string.IsNullOrEmpty(Conversion.Input))
        {
            Conversion.Input = "3.1415";
        }

        ViewData["ConversionType"] = ConversionTypes.All[ConversionTypes.MilesToKilometers];
        ViewData["Title"] = "Conversions";

        double inputValue;

        try
        {
            inputValue = Convert.ToDouble(Conversion.Input);
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

        string conversionType = Conversion.ConversionType;

        if (ConversionTypes.All.ContainsKey(conversionType))
        {
            DisplayConversionType = ConversionTypes.All[conversionType];
        }
        else if (string.Equals(
                     conversionType,
                     ConversionTypes.All[ConversionTypes.MilesToKilometers],
                     StringComparison.OrdinalIgnoreCase))
        {
            conversionType = ConversionTypes.MilesToKilometers;
            Conversion.ConversionType = conversionType;
            DisplayConversionType = ConversionTypes.All[conversionType];
        }
        else
        {
            ViewData["ErrorMessage"] = "Unknown conversion type.";
            return;
        }

        double result;

        try
        {
            result = conversionType switch
            {
                ConversionTypes.MilesToKilometers =>
                    new Length().FromMiles(inputValue).ToKilometers(),

                ConversionTypes.KilometersToMiles =>
                    new Length().FromKilometers(inputValue).ToMiles(),

                ConversionTypes.FahrenheitToCelsius =>
                    new Temperature().FromFahrenheit(inputValue).ToCelsius(),

                ConversionTypes.CelsiusToFahrenheit =>
                    new Temperature().FromCelsius(inputValue).ToFahrenheit(),

                ConversionTypes.PoundsToKilograms =>
                    new Mass().FromPounds(inputValue).ToKilograms(),

                ConversionTypes.KilogramsToPounds =>
                    new Mass().FromKilograms(inputValue).ToPounds(),

                ConversionTypes.MilesToFeet =>
                    new Length().FromMiles(inputValue).ToFeet(),

                ConversionTypes.FeetToMiles =>
                    new Length().FromFeet(inputValue).ToMiles(),

                _ => throw new InvalidOperationException()
            };
        }
        catch (InvalidOperationException)
        {
            ViewData["ErrorMessage"] = "Unknown conversion type.";
            return;
        }

        Conversion.Output = result.ToString();
    }
}
