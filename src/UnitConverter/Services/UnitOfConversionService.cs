
using UnitConverter.Models;
using UnitOf;

namespace UnitConverter.Services;

public class UnitOfConversionService : IConversionService
{
    public decimal Convert(decimal value, string conversionType)
    {
        double inputValue = (double)value;

        double result = conversionType switch
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

            _ => throw new InvalidOperationException("Unknown conversion type.")
        };

        return (decimal)result;
    }
}
