using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class TrustEvidenceCalculator
{
    public void Calculate(EnhancedResult result)
    {
        result.BehavioralScore = CalculateBehavioralScore(result);

        result.PerformanceScore = CalculatePerformanceScore(result);

        result.ActionScore = CalculateActionScore(result);

        result.SecurityScore = CalculateSecurityScore(result);
    }

    private double CalculateBehavioralScore(EnhancedResult result)
    {
        // Positive behavioral evidence:
        // the analyzer produced a structured finding
        // with a valid source location.

        double score = 0.50;

        if (!string.IsNullOrWhiteSpace(result.RuleId))
            score += 0.10;

        if (!string.IsNullOrWhiteSpace(result.Message))
            score += 0.10;

        if (result.StartLine > 0)
            score += 0.10;

        if (result.EndLine >= result.StartLine)
            score += 0.10;

        return Math.Clamp(score, 0.0, 1.0);
    }

    private double CalculatePerformanceScore(EnhancedResult result)
    {
        // Current performance evidence is based on
        // confidence and rule reliability.

        double score =
            (0.60 * result.ConfidenceScore) +
            (0.40 * result.RuleReliability);

        return Math.Clamp(score, 0.0, 1.0);
    }
private double CalculateActionScore(EnhancedResult result)
{
    double score = 0.50;

    // Successful action outcome increases trust evidence.
    if (result.ActionSucceeded)
        score += 0.20;
    else
        score -= 0.20;

    // Higher outcome quality provides stronger evidence.
    score += (result.OutcomeScore - 0.50) * 0.30;

    // Feedback from the action outcome contributes
    // additional adaptive evidence.
    score += (result.FeedbackScore - 0.50) * 0.20;

    return Math.Clamp(score, 0.0, 1.0);
}

    private double CalculateSecurityScore(EnhancedResult result)
    {
        double score = 0.50;

        if (!string.IsNullOrWhiteSpace(result.CWE) &&
            !result.CWE.Equals("Unknown",
                StringComparison.OrdinalIgnoreCase))
        {
            score += 0.15;
        }

        if (!string.IsNullOrWhiteSpace(result.Owasp) &&
            !result.Owasp.Equals("Unknown",
                StringComparison.OrdinalIgnoreCase))
        {
            score += 0.15;
        }

        if (result.Evidence.Count > 0)
            score += 0.10;

        if (result.IsVerified)
            score += 0.10;

        return Math.Clamp(score, 0.0, 1.0);
    }
}