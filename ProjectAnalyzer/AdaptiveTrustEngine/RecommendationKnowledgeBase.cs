namespace ProjectAnalyzer.AdaptiveTrustEngine.Models;

internal static class RecommendationKnowledgeBase
{
    private static readonly Dictionary<string, RecommendationInfo> KnowledgeBase = new()
    {
      ["SQLI001"] = new RecommendationInfo
{
    Title = "SQL Injection",

    Severity = "Critical",

    Risk = "Critical",

    CWE = "CWE-89",

    Owasp = "A03:2021 - Injection",

    Priority = "Immediate",

    EstimatedFixMinutes = 15,

    RuleReliability = 0.98,

    Recommendation =
        "Use parameterized SQL queries instead of string concatenation.",

    ExampleFix =
@"SqlCommand cmd = new SqlCommand(
""SELECT * FROM Users WHERE Id=@id"");

cmd.Parameters.AddWithValue(""@id"", userId);",

    Reference =
        "OWASP SQL Injection Prevention Cheat Sheet"
},

        ["PATH_TRAVERSAL_001"] = new RecommendationInfo
        {
            Title = "Path Traversal",
            Risk = "High",

            Recommendation =
                "Validate file paths, reject '../' sequences, and restrict access to approved directories.",

            ExampleFix =
@"string safePath =
Path.GetFullPath(userInput);

if(!safePath.StartsWith(baseDirectory))
    throw new Exception(""Invalid path"");",

            Reference =
                "OWASP Path Traversal"
        },

        ["HARD_CODED_CREDENTIALS_001"] = new RecommendationInfo
        {
            Title = "Hardcoded Credentials",
            Risk = "High",

            Recommendation =
                "Move credentials into environment variables or a secure secret manager.",

            ExampleFix =
@"var password =
Environment.GetEnvironmentVariable(""DB_PASSWORD"");",

            Reference =
                "OWASP Secrets Management"
        },

        ["XSS001"] = new RecommendationInfo
        {
            Title = "Cross Site Scripting",

            Risk = "High",

            Recommendation =
                "Encode output before rendering HTML and validate all user inputs.",

            ExampleFix =
@"@Html.Encode(userInput)",

            Reference =
                "OWASP XSS Prevention Cheat Sheet"
        },

        ["COMMAND_INJECTION_001"] = new RecommendationInfo
        {
            Title = "Command Injection",

            Risk = "Critical",

            Recommendation =
                "Never execute OS commands using raw user input.",

            ExampleFix =
@"// Avoid Process.Start(userInput)",

            Reference =
                "OWASP Command Injection"
        }
    };

    internal static RecommendationInfo Get(string ruleId)
    {
        if (KnowledgeBase.TryGetValue(ruleId, out var recommendation))
            return recommendation;

        return new RecommendationInfo
        {
            Title = "General Security Finding",
            Risk = "Medium",

            Recommendation =
                "Review the affected code and apply secure coding practices.",

            ExampleFix =
                "Refer to secure coding guidelines.",

            Reference =
                "OWASP Secure Coding Practices"
        };
    }
}