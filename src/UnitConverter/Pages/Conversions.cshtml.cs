
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using UnitConverter.Models;
using UnitOf;

namespace UnitConverter.Pages;

public class ConversionsModel : PageModel
{
    private readonly ILogger<ConversionsModel>? _logger;

    public ConversionsModel()
    {
        _logger = null;
    }

    [ActivatorUtilitiesConstructor]
    public ConversionsModel(ILogger<ConversionsModel> logger)
    {
        _logger = logger;
    }

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

        ViewData["ConversionType"] =
            ConversionTypes.All[ConversionTypes.MilesToKilometers];
        ViewData["Title"] = "Conversions";

        double inputValue;

        try
        {
            inputValue = Convert.ToDouble(Conversion.Input);
        }
        catch (FormatException ex)
        {
            _logger?.LogWarning(
                ex,
                "Invalid input {Input} for conversion {ConversionType}",
                Conversion.Input,
                Conversion.ConversionType);

            ViewData["ErrorMessage"] = "Input must be a valid number.";
            return;
        }
        catch (OverflowException ex)
        {
            _logger?.LogWarning(
                ex,
                "Input {Input} is outside the numeric range for {ConversionType}",
                Conversion.Input,
                Conversion.ConversionType);

            ViewData["ErrorMessage"] =
                "Input is outside the valid numeric range.";
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
            _logger?.LogWarning(
                "Unknown conversion type {ConversionType} for input {Input}",
                conversionType,
                Conversion.Input);

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
        catch (Exception ex)
        {
            _logger?.LogError(
                ex,
                "Conversion failed for input {Input} using {ConversionType}",
                inputValue,
                conversionType);

            ViewData["ErrorMessage"] =
                "An unexpected error occurred during conversion.";
            return;
        }

        Conversion.Output = result.ToString();

        _logger?.LogInformation(
            "Converted {Input} using {ConversionType} with result {Result}",
            inputValue,
            conversionType,
            result);
    }
}
