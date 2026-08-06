using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class TrustScoreCalculator
{
    public double Calculate(EnhancedResult result)
    {
        // -------------------------------
        // Confidence (40%)
        // -------------------------------
        double confidence =
            result.ConfidenceScore * 0.40;

        // -------------------------------
        // Rule Reliability (25%)
        // -------------------------------
        double reliability =
            result.RuleReliability * 0.25;

        // -------------------------------
        // Evidence Strength (20%)
        // -------------------------------
        double evidence =
            Math.Min(result.Evidence.Count, 5) / 5.0;

        evidence *= 0.20;

        // -------------------------------
        // Verification (15%)
        // -------------------------------
        double verification =
            result.IsVerified ? 0.15 : 0.05;

        double trust =
            confidence +
            reliability +
            evidence +
            verification;

        return Math.Min(trust, 1.0);
    }
}