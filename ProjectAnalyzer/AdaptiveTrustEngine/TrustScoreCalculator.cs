namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class TrustScoreCalculator
{
    public double Calculate(
        Models.EnhancedResult result)
    {
        // ==========================================
        // Evidence-based trust calculation
        // ==========================================

        double evidenceTrust =
            (0.25 * result.BehavioralScore) +
            (0.25 * result.PerformanceScore) +
            (0.20 * result.ActionScore) +
            (0.30 * result.SecurityScore);

        // ==========================================
        // Combine LLM confidence with evidence trust
        // ==========================================

        double trust =
            (0.40 * result.ConfidenceScore) +
            (0.60 * evidenceTrust);

        return Math.Clamp(trust, 0.0, 1.0);
    }
}