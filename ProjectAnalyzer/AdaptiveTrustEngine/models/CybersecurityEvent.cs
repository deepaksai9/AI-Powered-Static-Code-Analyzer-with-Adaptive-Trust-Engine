namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class CybersecurityEvent
{
    public string EventId { get; set; } = Guid.NewGuid().ToString();

    public string AgentId { get; set; } = "CAI-Agent";

    public string EventType { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}