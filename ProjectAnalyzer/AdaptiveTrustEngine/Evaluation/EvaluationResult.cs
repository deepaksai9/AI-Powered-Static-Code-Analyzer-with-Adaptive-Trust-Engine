namespace ProjectAnalizer.Evaluation;

internal class EvaluationResult
{
    public string DatasetName { get; set; } = "";

    public int Findings { get; set; }

    public double AverageConfidence { get; set; }

    public double AverageTrust { get; set; }

    public int Critical { get; set; }

    public int High { get; set; }

    public int Medium { get; set; }

    public int Low { get; set; }

    public double ExecutionTime { get; set; }
}