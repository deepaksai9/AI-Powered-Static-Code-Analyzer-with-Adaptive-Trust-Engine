namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class ActionOutcome
{
    public string OutcomeId { get; set; } = Guid.NewGuid().ToString();

    public string ActionId { get; set; } = "";

    public bool Success { get; set; }

    public double OutcomeScore { get; set; }

    public string Description { get; set; } = "";

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}