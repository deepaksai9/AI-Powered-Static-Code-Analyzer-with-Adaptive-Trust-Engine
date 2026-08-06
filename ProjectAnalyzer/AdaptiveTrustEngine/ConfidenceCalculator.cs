using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class ConfidenceCalculator
{
    public double Calculate(EnhancedResult result)
    {
        // -------------------------------
        // Rule Reliability (30%)
        // -------------------------------
        double reliability =
            result.RuleReliability * 0.30;

        // -------------------------------
        // Evidence Quality (25%)
        // Max contribution after 4 pieces
        // -------------------------------
        double evidence =
            Math.Min(result.Evidence.Count, 4) / 4.0;

        evidence *= 0.25;

        // -------------------------------
        // Severity (20%)
        // -------------------------------
        double severity = result.Severity.ToUpper() switch
        {
            "CRITICAL" => 0.20,
            "HIGH" => 0.18,
            "MEDIUM" => 0.14,
            "LOW" => 0.10,
            _ => 0.08
        };

        // -------------------------------
        // Verification (15%)
        // -------------------------------
        double verification =
            result.IsVerified ? 0.15 : 0.05;

        // -------------------------------
        // Knowledge Base Match (10%)
        // -------------------------------
        double knowledge =
            result.RuleId == "GENERIC001"
                ? 0.04
                : 0.10;

        double confidence =
            reliability +
            evidence +
            severity +
            verification +
            knowledge;

        return Math.Min(confidence, 1.0);
    }
}