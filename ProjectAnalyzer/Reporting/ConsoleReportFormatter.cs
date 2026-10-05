using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalizer.Reporting;

internal static class ConsoleReportFormatter
{
    public static void Print(EnhancedResult result)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 70));
        Console.WriteLine("              AI SECURITY ANALYSIS REPORT");
        Console.WriteLine(new string('=', 70));

        Console.WriteLine($"Title               : {result.Title}");
        Console.WriteLine($"Knowledge Rule ID   : {result.KnowledgeRuleId}");
        Console.WriteLine($"Original Rule ID    : {result.RuleId}");

        Console.WriteLine();

        Console.WriteLine($"Severity            : {result.Severity}");
        Console.WriteLine($"Priority            : {result.Priority}");

        Console.WriteLine();

        Console.WriteLine($"Confidence          : {result.ConfidenceScore:P0}");
        Console.WriteLine($"Trust Score         : {result.TrustScore:P0}");

        Console.WriteLine();

        // --------------------------------------------------
        // Risk-Aware Decision
        // --------------------------------------------------
        Console.WriteLine("Risk-Aware Decision");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(
            $"Action Risk         : {result.ActionRiskScore:P0}");

        Console.WriteLine(
            $"Action Decision     : {result.ActionDecision}");

        Console.WriteLine();

        // --------------------------------------------------
        // Controlled Security Action
        // --------------------------------------------------
        Console.WriteLine("Controlled Security Action");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(
            $"Control Status      : {result.ControlStatus}");

        Console.WriteLine(
            $"Action Allowed      : {result.ActionAllowed}");

        Console.WriteLine(
            $"Verification Req.   : {result.VerificationRequired}");

        Console.WriteLine(
            $"Control Description : {result.ControlDescription}");

        Console.WriteLine();

        // --------------------------------------------------
        // Adaptive Trust
        // --------------------------------------------------
        Console.WriteLine("Adaptive Trust");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(
            $"Previous Trust      : {result.PreviousTrustScore:P0}");

        Console.WriteLine(
            $"Updated Trust       : {result.UpdatedTrustScore:P0}");

        Console.WriteLine(
            $"Feedback Score      : {result.FeedbackScore:P0}");

        Console.WriteLine(
            $"Evaluation Count    : {result.EvaluationCount}");

        Console.WriteLine(
            $"Historical Trust    : {result.HasHistoricalTrust}");

        Console.WriteLine();

        // --------------------------------------------------
        // Action Outcome
        // --------------------------------------------------
        Console.WriteLine("Action Outcome");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(
            $"Action Succeeded    : {result.ActionSucceeded}");

        Console.WriteLine(
            $"Outcome Score       : {result.OutcomeScore:P0}");

        Console.WriteLine();

        // --------------------------------------------------
        // Feedback
        // --------------------------------------------------
        Console.WriteLine("Feedback");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(
            $"Feedback Score      : {result.FeedbackScore:P0}");

        Console.WriteLine(
            $"Feedback Source     : {result.FeedbackSource}");

        Console.WriteLine();

        // --------------------------------------------------
        // Trust Evidence
        // --------------------------------------------------
        Console.WriteLine("Trust Evidence");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(
            $"Behavior Score      : {result.BehavioralScore:P0}");

        Console.WriteLine(
            $"Performance Score   : {result.PerformanceScore:P0}");

        Console.WriteLine(
            $"Action Score        : {result.ActionScore:P0}");

        Console.WriteLine(
            $"Security Score      : {result.SecurityScore:P0}");

        Console.WriteLine();

        // --------------------------------------------------
        // Risk
        // --------------------------------------------------
        Console.WriteLine("Risk");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(result.Risk);

        Console.WriteLine();

        Console.WriteLine(
            $"CWE                 : {result.CWE}");

        Console.WriteLine(
            $"OWASP               : {result.Owasp}");

        Console.WriteLine();

        Console.WriteLine(
            $"Estimated Fix Time  : {result.EstimatedFixMinutes} minutes");

        Console.WriteLine();

        // --------------------------------------------------
        // Recommendation
        // --------------------------------------------------
        Console.WriteLine("Recommendation");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(result.Recommendation);

        Console.WriteLine();

        // --------------------------------------------------
        // Example Fix
        // --------------------------------------------------
        Console.WriteLine("Example Fix");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(result.ExampleFix);

        Console.WriteLine();

        // --------------------------------------------------
        // Reference
        // --------------------------------------------------
        Console.WriteLine("Reference");
        Console.WriteLine(new string('-', 70));

        Console.WriteLine(result.Reference);

        Console.WriteLine();

        // --------------------------------------------------
        // Evidence
        // --------------------------------------------------
        Console.WriteLine("Evidence");
        Console.WriteLine(new string('-', 70));

        foreach (var evidence in result.Evidence)
        {
            Console.WriteLine($"✓ {evidence}");
        }

        Console.WriteLine();
        Console.WriteLine(new string('=', 70));
    }
}