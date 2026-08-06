namespace ProjectAnalyzer.Benchmark.Models;

internal class BenchmarkResult
{
    public required string DatasetName { get; set; }

    public int FileCount { get; set; }

    public int FindingCount { get; set; }

    public double AverageConfidence { get; set; }

    public double AverageTrust { get; set; }

    public double ExecutionTimeSeconds { get; set; }

    public bool SarifGenerated { get; set; }

    public string Notes { get; set; } = string.Empty;
}