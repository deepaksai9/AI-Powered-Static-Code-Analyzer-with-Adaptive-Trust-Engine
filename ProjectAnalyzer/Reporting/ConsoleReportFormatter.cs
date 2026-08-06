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

        Console.WriteLine("Risk");
        Console.WriteLine(new string('-',70));
        Console.WriteLine(result.Risk);

        Console.WriteLine();

        Console.WriteLine($"CWE                 : {result.CWE}");
        Console.WriteLine($"OWASP               : {result.Owasp}");

        Console.WriteLine();

        Console.WriteLine($"Estimated Fix Time  : {result.EstimatedFixMinutes} minutes");

        Console.WriteLine();

        Console.WriteLine("Recommendation");
        Console.WriteLine(new string('-',70));
        Console.WriteLine(result.Recommendation);

        Console.WriteLine();

        Console.WriteLine("Example Fix");
        Console.WriteLine(new string('-',70));
        Console.WriteLine(result.ExampleFix);

        Console.WriteLine();

        Console.WriteLine("Reference");
        Console.WriteLine(new string('-',70));
        Console.WriteLine(result.Reference);

        Console.WriteLine();

        Console.WriteLine("Evidence");
        Console.WriteLine(new string('-',70));

        foreach (var evidence in result.Evidence)
            Console.WriteLine($"✓ {evidence}");

        Console.WriteLine();
        Console.WriteLine(new string('=',70));
    }
}