using ProjectAnalyzer.AdaptiveTrustEngine.Models;
namespace ProjectAnalizer.Evaluation;

internal static class MetricsCalculator
{
    public static EvaluationResult Calculate(
    string dataset,
    IEnumerable<EnhancedResult> results,
    double executionTime)    {
        var list = results.ToList();

        return new EvaluationResult
        {
            DatasetName = dataset,

            Findings = list.Count,

            AverageConfidence =
                list.Any()
                    ? list.Average(x => x.ConfidenceScore)
                    : 0,

            AverageTrust =
                list.Any()
                    ? list.Average(x => x.TrustScore)
                    : 0,

            Critical =
                list.Count(x =>
                    x.Severity.Equals("Critical",
                        StringComparison.OrdinalIgnoreCase)),

            High =
                list.Count(x =>
                    x.Severity.Equals("High",
                        StringComparison.OrdinalIgnoreCase)),

            Medium =
                list.Count(x =>
                    x.Severity.Equals("Medium",
                        StringComparison.OrdinalIgnoreCase)),

            Low =
                list.Count(x =>
                    x.Severity.Equals("Low",
                        StringComparison.OrdinalIgnoreCase)),

            ExecutionTime = executionTime
        };
    }
}