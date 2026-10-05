using System.Globalization;
using System.Text;
using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal static class EvaluationCsvExporter
{
    public static void Append(
        string outputFile,
        string projectName,
        string model,
        IReadOnlyCollection<EnhancedResult> results)
    {
        if (results.Count == 0)
        {
            return;
        }

        bool fileExists = File.Exists(outputFile);

        var csv = new StringBuilder();

        if (!fileExists)
        {
            csv.AppendLine(
                "Timestamp,Project,Model,Findings," +
                "AvgConfidence,AvgTrust,AvgRisk," +
                "Allow,Verify,Restrict,Block," +
                "AvgFeedback,InitialTrust,FinalTrust,TrustChange," +
                "Evaluations");
        }

        double averageConfidence =
            results.Average(x => x.ConfidenceScore);

        double averageTrust =
            results.Average(x => x.TrustScore);

        double averageRisk =
            results.Average(x => x.ActionRiskScore);

        double averageFeedback =
            results.Average(x => x.FeedbackScore);

        int allowCount =
            results.Count(x =>
                x.ActionDecision.Equals(
                    "Allow",
                    StringComparison.OrdinalIgnoreCase));

        int verifyCount =
            results.Count(x =>
                x.ActionDecision.Equals(
                    "Verify",
                    StringComparison.OrdinalIgnoreCase));

        int restrictCount =
            results.Count(x =>
                x.ActionDecision.Equals(
                    "Restrict",
                    StringComparison.OrdinalIgnoreCase));

        int blockCount =
            results.Count(x =>
                x.ActionDecision.Equals(
                    "Block",
                    StringComparison.OrdinalIgnoreCase));

        double initialTrust =
            results.First().PreviousTrustScore;

        double finalTrust =
            results.Last().UpdatedTrustScore;

        double trustChange =
            finalTrust - initialTrust;

        int evaluations =
            results.Last().EvaluationCount + 1;

        string timestamp =
            DateTime.UtcNow.ToString(
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture);

        csv.AppendLine(
            string.Join(
                ",",
                timestamp,
                Escape(projectName),
                Escape(model),
                results.Count.ToString(
                    CultureInfo.InvariantCulture),

                ToPercent(averageConfidence),
                ToPercent(averageTrust),
                ToPercent(averageRisk),

                allowCount.ToString(
                    CultureInfo.InvariantCulture),

                verifyCount.ToString(
                    CultureInfo.InvariantCulture),

                restrictCount.ToString(
                    CultureInfo.InvariantCulture),

                blockCount.ToString(
                    CultureInfo.InvariantCulture),

                ToPercent(averageFeedback),
                ToPercent(initialTrust),
                ToPercent(finalTrust),
                ToSignedPercent(trustChange),

                evaluations.ToString(
                    CultureInfo.InvariantCulture)));
        
        File.AppendAllText(
            outputFile,
            csv.ToString());
    }

    private static string ToPercent(double value)
    {
        return
            (value * 100.0)
            .ToString("F2", CultureInfo.InvariantCulture);
    }

    private static string ToSignedPercent(double value)
    {
        return
            (value * 100.0)
            .ToString("+0.00;-0.00;0.00", CultureInfo.InvariantCulture);
    }

    private static string Escape(string value)
    {
        if (value.Contains(',') ||
            value.Contains('"') ||
            value.Contains('\n') ||
            value.Contains('\r'))
        {
            return "\"" +
                   value.Replace("\"", "\"\"") +
                   "\"";
        }

        return value;
    }
}