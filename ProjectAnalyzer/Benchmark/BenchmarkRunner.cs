using System.Diagnostics;
using ProjectAnalyzer.Benchmark.Models;

namespace ProjectAnalyzer.Benchmark;

internal class BenchmarkRunner
{
    public async Task<List<BenchmarkResult>> RunAsync(
        string datasetsRoot,
        Func<string, Task<BenchmarkResult>> analyzer)
    {
        var results = new List<BenchmarkResult>();

        var datasets = Directory.GetDirectories(datasetsRoot)
            .OrderBy(x => x);

        foreach (var dataset in datasets)
        {
            Console.WriteLine();
            Console.WriteLine($"====================================");
            Console.WriteLine($"Analyzing {Path.GetFileName(dataset)}");
            Console.WriteLine($"====================================");

            var stopwatch = Stopwatch.StartNew();

            var result = await analyzer(dataset);

            stopwatch.Stop();

            result.ExecutionTimeSeconds = stopwatch.Elapsed.TotalSeconds;

            results.Add(result);
        }

        return results;
    }
}