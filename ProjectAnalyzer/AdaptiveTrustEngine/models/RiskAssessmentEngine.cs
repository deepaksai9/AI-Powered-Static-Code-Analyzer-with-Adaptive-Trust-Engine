namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class RiskAssessmentEngine
{
    public string GetRisk(double trustScore)
    {
        if (trustScore >= 0.90)
            return "Critical";

        if (trustScore >= 0.75)
            return "High";

        if (trustScore >= 0.50)
            return "Medium";

        return "Low";
    }

    public string GetPriority(string risk)
    {
        return risk switch
        {
            "Critical" => "Immediate",
            "High" => "Within 24 Hours",
            "Medium" => "Next Sprint",
            _ => "Routine"
        };
    }

    public int EstimatedFixMinutes(string risk)
    {
        return risk switch
        {
            "Critical" => 15,
            "High" => 30,
            "Medium" => 60,
            _ => 120
        };
    }
}