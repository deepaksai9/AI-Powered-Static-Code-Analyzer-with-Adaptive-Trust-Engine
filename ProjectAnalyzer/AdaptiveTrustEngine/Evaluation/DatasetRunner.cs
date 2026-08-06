using System.Diagnostics;

namespace ProjectAnalizer.Evaluation;

internal class DatasetRunner
{
    public async Task RunAsync(
        Func<string, Task> analyzer,
        string datasetsRoot)
    {
        if (!Directory.Exists(datasetsRoot))
        {
            Console.WriteLine($"Dataset folder not found: {datasetsRoot}");
            return;
        }

        var datasets = Directory
            .GetDirectories(datasetsRoot)
            .OrderBy(x => x)
            .ToList();

        Console.WriteLine();
        Console.WriteLine("=========================================");
        Console.WriteLine("      DATASET EVALUATION STARTED");
        Console.WriteLine("=========================================");
        Console.WriteLine();

        foreach (var dataset in datasets)
        {
            Console.WriteLine($"Running {Path.GetFileName(dataset)}");

            var clock = Stopwatch.StartNew();

            await analyzer(dataset);

            clock.Stop();

            Console.WriteLine(
                $"Completed in {clock.Elapsed.TotalSeconds:F2} sec");

            Console.WriteLine("-----------------------------------------");
        }

        Console.WriteLine();
        Console.WriteLine("=========================================");
        Console.WriteLine("      ALL DATASETS COMPLETED");
        Console.WriteLine("=========================================");
    }
}