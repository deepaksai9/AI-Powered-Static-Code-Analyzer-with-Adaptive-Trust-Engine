namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class EnhancedResult
{
    // Original LLM Result

    public required string RuleId { get; set; }

    public required string RuleDescription { get; set; }

    public required string Message { get; set; }

    public required string Path { get; set; }

    public required string Category { get; set; }

    public required int StartLine { get; set; }

    public required int EndLine { get; set; }

    // Adaptive Trust Engine

    public double ConfidenceScore { get; set; }

    public double TrustScore { get; set; }

    public bool IsVerified { get; set; }

    public List<string> Evidence { get; set; } = new();

    // Recommendation Information

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
    public string KnowledgeRuleId { get; set; } = "";
}