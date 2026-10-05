namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class AgentTrustState
{
    public string AgentId { get; set; } = "CAI-Agent";

    public double TrustScore { get; set; } = 0.50;

    public int EvaluationCount { get; set; }

    public int SuccessfulActions { get; set; }

    public int FailedActions { get; set; }

    public double AverageFeedback { get; set; } = 0.50;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}