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
        string search =
            $"{result.RuleId} " +
            $"{result.RuleDescription} " +
            $"{result.Message} " +
            $"{result.Category}"
            .ToLowerInvariant();

        // ---------- SQL Injection ----------
        if (search.Contains("sql injection") ||
            search.Contains("sqli") ||
            search.Contains("database injection") ||
            search.Contains("sql query") ||
            search.Contains("sql command") ||
            search.Contains("sql concatenation"))
        {
            return "SQL Injection";
        }

        // ---------- Hardcoded Credentials ----------
        bool credentialTerm =
            search.Contains("credential") ||
            search.Contains("password") ||
            search.Contains("secret") ||
            search.Contains("api key") ||
            search.Contains("apikey") ||
            search.Contains("token");

        bool hardcodedIndicator =
            search.Contains("hardcoded") ||
            search.Contains("hard-coded") ||
            search.Contains("embedded") ||
            search.Contains("stored directly");

        if (credentialTerm && hardcodedIndicator)
        {
            return "Hardcoded Credentials";
        }

        // ---------- Insecure Deserialization ----------
        if (search.Contains("insecure deserialization") ||
            search.Contains("unsafe deserialization") ||
            search.Contains("deserializing untrusted") ||
            search.Contains("deserialization vulnerability"))
        {
            return "Insecure Deserialization";
        }

        // ---------- Path Traversal ----------
        if (search.Contains("path traversal") ||
            search.Contains("directory traversal") ||
            search.Contains("file handling") ||
            search.Contains("user-controlled file path"))
        {
            return "Path Traversal";
        }

        // ---------- Cross-Site Scripting ----------
        if (search.Contains("xss") ||
            search.Contains("cross-site scripting") ||
            search.Contains("cross site scripting") ||
            search.Contains("html injection"))
        {
            return "Cross-Site Scripting";
        }

        // ---------- Command Injection ----------
        if (search.Contains("command injection") ||
            search.Contains("cmd injection") ||
            search.Contains("shell command") ||
            search.Contains("process execution"))
        {
            return "Command Injection";
        }

        // ---------- Weak Cryptography ----------
        if (search.Contains("cryptography") ||
            search.Contains("cryptographic") ||
            search.Contains("weak crypto") ||
            search.Contains("des encryption") ||
            search.Contains("weak encryption"))
        {
            return "Weak Cryptography";
        }

        return result.Category;
    }
}