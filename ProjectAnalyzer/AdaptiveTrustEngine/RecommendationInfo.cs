namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class RecommendationInfo
{
    public string Title { get; set; } = "";

    public string Severity { get; set; } = "";

    public string Risk { get; set; } = "";

    public string CWE { get; set; } = "";

    public string Owasp { get; set; } = "";

    public string Priority { get; set; } = "";

    public int EstimatedFixMinutes { get; set; }

    public double RuleReliability { get; set; }

    public string Recommendation { get; set; } = "";

    public string ExampleFix { get; set; } = "";

    public string Reference { get; set; } = "";
}