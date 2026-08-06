using ProjectAnalyzer.AdaptiveTrustEngine.KnowledgeBase;
using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class RecommendationEngine
{
    public VulnerabilityInfo GetKnowledge(EnhancedResult result)
    {
        return VulnerabilityKnowledge.Get(BuildSearchKey(result));
    }

    public string GetRecommendation(EnhancedResult result)
    {
        return GetKnowledge(result).Recommendation;
    }

    private static string BuildSearchKey(EnhancedResult result)
    {
        var search =
            $"{result.RuleId} {result.RuleDescription} {result.Category}"
            .ToLowerInvariant();

        // ---------- SQL Injection ----------
        if (search.Contains("sql") ||
            search.Contains("sqli") ||
            search.Contains("database injection"))
        {
            return "SQL Injection";
        }

        // ---------- Hardcoded Credentials ----------
        if (search.Contains("credential") ||
            search.Contains("password") ||
            search.Contains("secret") ||
            search.Contains("api key") ||
            search.Contains("apikey") ||
            search.Contains("token"))
        {
            return "Hardcoded Credentials";
        }

        // ---------- Path Traversal ----------
        if (search.Contains("path") ||
            search.Contains("traversal") ||
            search.Contains("directory traversal") ||
            search.Contains("file handling"))
        {
            return "Path Traversal";
        }

        // ---------- Cross Site Scripting ----------
        if (search.Contains("xss") ||
            search.Contains("cross-site scripting") ||
            search.Contains("cross site scripting") ||
            search.Contains("html injection"))
        {
            return "Cross-Site Scripting";
        }

        // ---------- Command Injection ----------
        if (search.Contains("command injection") ||
            search.Contains("cmd injection"))
        {
            return "Command Injection";
        }

        return result.Category;
    }
}