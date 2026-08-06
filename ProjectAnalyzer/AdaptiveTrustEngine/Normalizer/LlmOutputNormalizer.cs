using ProjectAnalyzer.Dtos;

namespace ProjectAnalyzer.AdaptiveTrustEngine.Normalizer;

internal class LlmOutputNormalizer
{
    public IEnumerable<LlmResponseDto.ResultDto> Normalize(
        IEnumerable<LlmResponseDto.ResultDto> results)
    {
        foreach (var result in results)
        {
            NormalizeSeverity(result);
            NormalizeCategory(result);
            NormalizeRuleId(result);
            NormalizePath(result);
        }

        return results;
    }

    private static void NormalizeSeverity(LlmResponseDto.ResultDto result)
    {
        result.Level = result.Level switch
        {
            SarifDto.SeverityLevel.Error => SarifDto.SeverityLevel.Error,
            SarifDto.SeverityLevel.Warning => SarifDto.SeverityLevel.Warning,
            SarifDto.SeverityLevel.Note => SarifDto.SeverityLevel.Note,
            _ => SarifDto.SeverityLevel.Warning
        };
    }

    private static void NormalizeCategory(LlmResponseDto.ResultDto result)
    {
        var category = result.Category.Trim().ToLowerInvariant();

        if (category.Contains("sql"))
            result.Category = "SQL Injection";

        else if (category.Contains("credential"))
            result.Category = "Hardcoded Credentials";

        else if (category.Contains("path") || category.Contains("file"))
            result.Category = "Path Traversal";

        else if (category.Contains("xss"))
            result.Category = "Cross-Site Scripting";

        else if (category.Contains("command"))
            result.Category = "Command Injection";
    }

    private static void NormalizeRuleId(LlmResponseDto.ResultDto result)
    {
        result.RuleId = result.Category switch
        {
            "SQL Injection" => "SQLI001",

            "Hardcoded Credentials" => "HCRED001",

            "Path Traversal" => "PATH001",

            "Cross-Site Scripting" => "XSS001",

            "Command Injection" => "CMDI001",

            _ => result.RuleId
        };
    }

    private static void NormalizePath(LlmResponseDto.ResultDto result)
    {
        result.Path = result.Path.Replace("\\", "/");
    }
}