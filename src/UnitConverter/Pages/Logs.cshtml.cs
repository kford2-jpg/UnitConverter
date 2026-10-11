using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnitConverter.Models;
using UnitConverter.Services;

namespace UnitConverter.Pages;

public class LogsModel : PageModel
{
    private readonly ILogReader _logReader;

    public LogsModel(ILogReader logReader)
    {
        _logReader = logReader;
    }

    public List<LogEntry> Entries { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Level { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string ConversionType { get; set; } = string.Empty;

    public void OnGet()
    {
        IEnumerable<LogEntry> entries = _logReader.Read();
        var filteredEntries = new List<LogEntry>();

        foreach (LogEntry entry in entries)
        {
            if (!string.IsNullOrEmpty(Level) &&
                entry.Level != Level)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(ConversionType) &&
                entry.ConversionType != ConversionType)
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(Search))
            {
                string searchText = Search.Trim();

                bool matches =
                    (entry.Message?.Contains(searchText,
                        StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (entry.ConversionType?.Contains(searchText,
                        StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (entry.Input?.ToString()?.Contains(searchText,
                        StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (entry.Result?.ToString()?.Contains(searchText,
                        StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (entry.Exception?.Contains(searchText,
                        StringComparison.OrdinalIgnoreCase) ?? false);

                if (!matches)
                {
                    continue;
                }
            }

            filteredEntries.Add(entry);
        }

        Entries = filteredEntries;
    }
}
