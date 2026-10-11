using Serilog;
using Serilog.Formatting.Compact;
using UnitConverter.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog.
string logPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "Logs",
    "unitconverter-.json");

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.File(
            new RenderedCompactJsonFormatter(),
            logPath,
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7);
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IConversionService, UnitOfConversionService>();
builder.Services.AddSingleton<ILogReader, JsonLogReader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();

/// <summary>
/// This is provided to support the WebApplicationFactory used in testing.
/// </summary>
public partial class Program
{
}
