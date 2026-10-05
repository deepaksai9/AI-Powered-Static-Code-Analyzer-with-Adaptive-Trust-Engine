namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class ControlledActionResult
{
    public string ActionId { get; set; } = "";

    public string Status { get; set; } = "";

    public bool Allowed { get; set; }

    public bool VerificationRequired { get; set; }

    public string Description { get; set; } = "";

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
