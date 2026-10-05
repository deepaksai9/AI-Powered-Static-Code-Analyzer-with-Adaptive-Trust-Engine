using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ProjectAnalyzer.Dtos;

namespace ProjectAnalizer.Analyzer;

internal class OllamaClient
{
    private readonly HttpClient _client;
    private static readonly Stopwatch Clock = new();

    public OllamaClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<LlmResponseDto?> AnalyzeAsync(
        string model,
        string userPrompt,
        CancellationToken cancellationToken = default)
    {
        Clock.Restart();

        var request = new
        {
            model,
            stream = false,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = Constants.SystemPrompt
                },
                new
                {
                    role = "user",
                    content = userPrompt
                }
            }
        };

        var response = await _client.PostAsJsonAsync(
            "http://localhost:11434/api/chat",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(
            cancellationToken);

        using var doc = JsonDocument.Parse(json);

        var content = doc.RootElement
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;

        Clock.Stop();

        Console.WriteLine("========== OLLAMA RESPONSE ==========");
        Console.WriteLine(content);
        Console.WriteLine("=====================================");

        string jsonContent;

        var match = Constants.JsonRegex().Match(content);

        if (match.Success)
        {
            jsonContent = match.Groups[1].Value;
        }
        else
        {
            jsonContent = content.Trim();
        }

        Console.WriteLine("========== JSON TO PARSE ==========");
        Console.WriteLine(jsonContent);
        Console.WriteLine("===================================");

        try
        {
            // Normalize severity values returned by the LLM
            jsonContent = jsonContent
                .Replace("\"Critical\"", "\"Error\"")
                .Replace("\"High\"", "\"Error\"")
                .Replace("\"Medium\"", "\"Warning\"")
                .Replace("\"Low\"", "\"Note\"");

            // Repair invalid JSON escape sequences returned by the LLM.
            // Example:
            // ".config\dotnet-tools.json"
            jsonContent = RepairInvalidJsonEscapes(jsonContent);

            var dto =
                JsonSerializer.Deserialize<
                    IEnumerable<LlmResponseDto.ResultDto>>(
                        jsonContent,
                        Constants.ChatJsonOptions);

            if (dto == null)
            {
                Console.WriteLine(
                    "Failed to deserialize LLM response.");

                return null;
            }

            Console.WriteLine(
                $"{model} succeeded - " +
                $"{Clock.Elapsed.TotalSeconds:F2} seconds");

            return new LlmResponseDto
            {
                Results = dto
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("========== PARSE ERROR ==========");
            Console.WriteLine(ex);
            Console.WriteLine("=================================");

            return null;
        }
    }

    private static string RepairInvalidJsonEscapes(string json)
    {
        var builder = new StringBuilder();

        for (int i = 0; i < json.Length; i++)
        {
            char current = json[i];

            if (current == '\\' && i + 1 < json.Length)
            {
                char next = json[i + 1];

                bool validEscape =
                    next == '"' ||
                    next == '\\' ||
                    next == '/' ||
                    next == 'b' ||
                    next == 'f' ||
                    next == 'n' ||
                    next == 'r' ||
                    next == 't';

                bool validUnicodeEscape =
                    next == 'u' &&
                    i + 5 < json.Length &&
                    IsHex(json[i + 2]) &&
                    IsHex(json[i + 3]) &&
                    IsHex(json[i + 4]) &&
                    IsHex(json[i + 5]);

                if (!validEscape && !validUnicodeEscape)
                {
                    // Convert an invalid escape such as \d
                    // into a literal backslash.
                    builder.Append("\\\\");
                    continue;
                }
            }

            builder.Append(current);
        }

        return builder.ToString();
    }

    private static bool IsHex(char c)
    {
        return
            (c >= '0' && c <= '9') ||
            (c >= 'a' && c <= 'f') ||
            (c >= 'A' && c <= 'F');
    }
}