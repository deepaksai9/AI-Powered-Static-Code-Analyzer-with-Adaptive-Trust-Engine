namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class SecurityAction
{
    public string ActionId { get; set; } = Guid.NewGuid().ToString();

    public string AgentId { get; set; } = "CAI-Agent";

    public string EventId { get; set; } = "";

    public string ActionType { get; set; } = "";

    public string Description { get; set; } = "";

    public double RiskScore { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}