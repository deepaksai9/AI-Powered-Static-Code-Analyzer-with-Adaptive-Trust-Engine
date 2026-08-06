using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;
using ProjectAnalizer.Reporting;
using ProjectAnalyzer.Dtos;
using ProjectAnalyzer.AdaptiveTrustEngine;
using ProjectAnalyzer.AdaptiveTrustEngine.Models;
using ProjectAnalizer.Analyzer;

namespace ProjectAnalizer;

internal class ProjectAnalyzer
{
    private static readonly Stopwatch Clock = new();
    internal static async Task AnalyzeAsync(
    IReadOnlyCollection<string> models,
    string token,
    string endpoint,
    string outputFileName,
    string? projectPath,
    IReadOnlyCollection<string> extensionsToAnalyze,
    IReadOnlyCollection<string> directoriesToskip,
    CancellationToken cancellationToken = default)
    {
string rootPath = projectPath ?? Directory.GetCurrentDirectory();
using var client = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(10)
};

var pipeline = new AnalysisPipeline(client);

        var sarif = new SarifDto
        {
            Schema = Constants.SarifSchema,
            Version = Constants.SarifVersion,
            Runs = []
        };

        foreach (var model in models)
{
    var ollama = new OllamaClient(client);
var dto = await pipeline.AnalyzeProjectAsync(
    model,
    rootPath,
    extensionsToAnalyze,
    directoriesToskip,
    cancellationToken);


    if (dto is null)
    {
        Console.WriteLine($"{model} skip.");
        continue;
    }

    // Run our Adaptive Trust Engine
    var trustEngine = new AdaptiveTrustEngine();
    var enhancedResults = trustEngine.Process(dto.Results);

    // Display enhanced results
    foreach (var result in enhancedResults)
{
    ConsoleReportFormatter.Print(result);
}
    // Keep original SARIF generation (baseline)
    var rules = dto.Results
        .GroupBy(x => x.RuleId)
        .Select(MapToRule())
        .ToList();
var results = enhancedResults
    .Select(MapToResult(rules))
    .ToList();

    sarif.Runs.Add(new SarifDto.RunDto
    {
        Tool = new SarifDto.ToolDto
        {
            Driver = new SarifDto.DriverDto
            {
                Name = model,
                SemanticVersion = "1.0.0",
                Version = "1.0.0",
                InformationUri = endpoint,
                Rules = rules
            }
        },
        Results = results,
    });

    Console.WriteLine($"{model} done.");
}

        var content = JsonSerializer.Serialize(sarif, Constants.SarifJsonOptions);
        await File.WriteAllTextAsync(outputFileName, content, cancellationToken);
    }

    private static Func<IGrouping<string, LlmResponseDto.ResultDto>, SarifDto.RuleDto> MapToRule()
    {
        return x => new SarifDto.RuleDto
        {
            Id = x.Key,
            DefaultConfiguration = new()
            {
                Level = x.Select(x => x.Level)
                    .GroupBy(r => r)
                    .OrderByDescending(r => r.Count())
                    .First()
                    .First()
            },
            Help = new()
            {
                Text = string.Empty
            },
            Properties = new()
            {
                Categories = [
                    x.Select(x => x.Category)
                        .GroupBy(r => r)
                        .OrderByDescending(r => r.Count())
                        .First()
                        .First()
                ],
                Tags = []
            },
            ShortDescription = new()
            {
                Text = x.First().RuleDescription
            }
        };
    }

    private static Func<EnhancedResult, SarifDto.ResultDto> MapToResult(List<SarifDto.RuleDto> rules)
{
    return x =>
    {
        var matchedRule = rules.FirstOrDefault(r => r.Id == x.RuleId);

        return new SarifDto.ResultDto
        {
            RuleId = x.RuleId,

            RuleIndex = matchedRule == null
                ? 0
                : rules.IndexOf(matchedRule),

            Level = SarifDto.SeverityLevel.Warning,

            Message = new()
            {
                Text = x.Message
            },

            Locations =
            [
                new()
                {
                    PhysicalLocation = new()
                    {
                        ArtifactLocation = new()
                        {
                            Uri = x.Path,
                            UriBaseId = "solutionDir"
                        },

                        Region = new()
                        {
                            StartLine = x.StartLine,
                            EndLine = x.EndLine,
                            StartColumn = 1,
                            EndColumn = 1
                        }
                    }
                }
            ],

            ConfidenceScore = x.ConfidenceScore,
            TrustScore = x.TrustScore,
            IsVerified = x.IsVerified,
            Recommendation = x.Recommendation,
            Evidence = x.Evidence,
            Priority = GetPriority(x.TrustScore)
        };
    };
}
private static string GetPriority(double trustScore)
{
    if (trustScore >= 0.90)
        return "Critical";

    if (trustScore >= 0.75)
        return "High";

    if (trustScore >= 0.50)
        return "Medium";

    return "Low";
}

    
    private static async Task LogAsync(string message, CancellationToken cancellationToken)
    {
        await File.AppendAllTextAsync("logs.txt", $"{DateTime.Now}: {message}\n", cancellationToken);
    }
}