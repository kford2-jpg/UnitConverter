using System.Text.Json;
using UnitConverter.Models;

namespace UnitConverter.Services;

public class JsonLogReader : ILogReader
{
    private readonly string _logDirectory;

    public JsonLogReader(IWebHostEnvironment environment)
    {
        _logDirectory = Path.Combine(
            environment.ContentRootPath,
            "Logs");
    }

    public IEnumerable<LogEntry> Read()
    {
        var entries = new List<LogEntry>();

        if (!Directory.Exists(_logDirectory))
        {
            return entries;
        }

        string[] logFiles;

        try
        {
            logFiles = Directory.GetFiles(_logDirectory, "*.json");
        }
        catch (IOException)
        {
            return entries;
        }
        catch (UnauthorizedAccessException)
        {
            return entries;
        }

        foreach (string logFile in logFiles)
        {
            string[] lines;

            try
            {
                lines = File.ReadAllLines(logFile);
            }
            catch (IOException)
            {
                continue;
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }

            foreach (string line in lines)
            {
                LogEntry? entry;

                try
                {
                    entry = JsonSerializer.Deserialize<LogEntry>(line);
                }
                catch (JsonException)
                {
                    continue;
                }

                if (entry != null)
                {
                    if (string.IsNullOrEmpty(entry.Level))
                    {
                        entry.Level = "Information";
                    }

                    entries.Add(entry);
                }
            }
        }

        return entries;
    }
}
