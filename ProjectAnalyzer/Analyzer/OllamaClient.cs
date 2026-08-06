using System.Diagnostics;
using System.Net.Http.Json;
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

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

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

            var dto = JsonSerializer.Deserialize<IEnumerable<LlmResponseDto.ResultDto>>(
                jsonContent,
                Constants.ChatJsonOptions);

            if (dto == null)
            {
                Console.WriteLine("Failed to deserialize LLM response.");
                return null;
            }

            Console.WriteLine($"{model} succeeded - {Clock.Elapsed.TotalSeconds:F2} seconds");

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
}