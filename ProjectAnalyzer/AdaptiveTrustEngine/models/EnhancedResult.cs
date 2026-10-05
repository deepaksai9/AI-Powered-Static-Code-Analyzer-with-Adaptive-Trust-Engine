namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal class EnhancedResult
{
    // ==============================
    // Original LLM Result
    // ==============================

    public required string RuleId { get; set; }

    public required string RuleDescription { get; set; }

    public required string Message { get; set; }

    public required string Path { get; set; }

    public required string Category { get; set; }

    public required int StartLine { get; set; }

    public required int EndLine { get; set; }


    // ==============================
    // Adaptive Trust Engine
    // ==============================

    public double ConfidenceScore { get; set; }

    public double TrustScore { get; set; }

    public bool IsVerified { get; set; }

    public List<string> Evidence { get; set; } = new();


    // ==============================
    // Dynamic Trust
    // ==============================

    // Trust value from the previous evaluation
    public double PreviousTrustScore { get; set; } = 0.50;

    // Feedback received after an action
    public double FeedbackScore { get; set; }

    // Trust after applying current evidence and feedback
    public double UpdatedTrustScore { get; set; }

    // Number of previous evaluations for this agent
    public int EvaluationCount { get; set; }

    // Indicates whether historical trust information was available
    public bool HasHistoricalTrust { get; set; }


    // ==============================
    // Trust Evidence
    // ==============================

    public double BehavioralScore { get; set; }

    public double PerformanceScore { get; set; }

    public double ActionScore { get; set; }

    public double SecurityScore { get; set; }


    // ==============================
    // Recommendation Information
    // ==============================

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


    // ==============================
    // Risk-Aware Action Decision
    // ==============================

    public string ActionDecision { get; set; } = "";

    public double ActionRiskScore { get; set; }
    public string ControlStatus { get; set; } = "";

public bool ActionAllowed { get; set; }

public bool VerificationRequired { get; set; }

public string ControlDescription { get; set; } = "";


    // ==============================
    // Action Outcome
    // ==============================

    public bool ActionSucceeded { get; set; }

    public double OutcomeScore { get; set; }


    // ==============================
    // Feedback
    // ==============================

    public string FeedbackSource { get; set; } = "";
}