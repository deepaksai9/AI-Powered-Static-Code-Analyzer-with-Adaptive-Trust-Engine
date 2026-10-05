namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class Feedback
{
    public string FeedbackId { get; set; } = Guid.NewGuid().ToString();

    public string ActionId { get; set; } = "";

    public double FeedbackScore { get; set; }

    public string Source { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}