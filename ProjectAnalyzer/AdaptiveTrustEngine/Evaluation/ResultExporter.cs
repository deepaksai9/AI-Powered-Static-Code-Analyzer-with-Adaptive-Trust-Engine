namespace ProjectAnalizer.Evaluation;

internal static class ResultExporter
{
    public static void Print(EvaluationResult result)
    {
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine($"Dataset             : {result.DatasetName}");
        Console.WriteLine($"Findings            : {result.Findings}");
        Console.WriteLine($"Average Confidence  : {result.AverageConfidence:P2}");
        Console.WriteLine($"Average Trust       : {result.AverageTrust:P2}");
        Console.WriteLine($"Critical            : {result.Critical}");
        Console.WriteLine($"High                : {result.High}");
        Console.WriteLine($"Medium              : {result.Medium}");
        Console.WriteLine($"Low                 : {result.Low}");
        Console.WriteLine($"Execution Time      : {result.ExecutionTime:F2} sec");
        Console.WriteLine("========================================");
    }

    public static void PrintSummary(IEnumerable<EvaluationResult> results)
    {
        var list = results.ToList();

        Console.WriteLine();
        Console.WriteLine("============= FINAL SUMMARY =============");

        Console.WriteLine($"Datasets            : {list.Count}");
        Console.WriteLine($"Total Findings      : {list.Sum(x => x.Findings)}");

        Console.WriteLine($"Average Confidence  : {list.Average(x => x.AverageConfidence):P2}");
        Console.WriteLine($"Average Trust       : {list.Average(x => x.AverageTrust):P2}");

        Console.WriteLine($"Critical Findings   : {list.Sum(x => x.Critical)}");
        Console.WriteLine($"High Findings       : {list.Sum(x => x.High)}");
        Console.WriteLine($"Medium Findings     : {list.Sum(x => x.Medium)}");
        Console.WriteLine($"Low Findings        : {list.Sum(x => x.Low)}");

        Console.WriteLine($"Total Time          : {list.Sum(x => x.ExecutionTime):F2} sec");

        Console.WriteLine("=========================================");
    }
}