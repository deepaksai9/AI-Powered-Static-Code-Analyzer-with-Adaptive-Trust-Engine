using System.Text;
using ProjectAnalyzer.Benchmark.Models;

namespace ProjectAnalyzer.Benchmark;

internal static class BenchmarkReportGenerator
{
    public static async Task GenerateCsvAsync(
        IEnumerable<BenchmarkResult> results,
        string outputFile)
    {
        var sb = new StringBuilder();

        sb.AppendLine(
            "Dataset,Files,Findings,AverageConfidence,AverageTrust,ExecutionTimeSeconds,SarifGenerated,Notes");

        foreach (var r in results)
        {
            sb.AppendLine(
                $"{r.DatasetName}," +
                $"{r.FileCount}," +
                $"{r.FindingCount}," +
                $"{r.AverageConfidence:F2}," +
                $"{r.AverageTrust:F2}," +
                $"{r.ExecutionTimeSeconds:F2}," +
                $"{r.SarifGenerated}," +
                $"\"{r.Notes}\"");
        }

        await File.WriteAllTextAsync(outputFile, sb.ToString());
    }
}