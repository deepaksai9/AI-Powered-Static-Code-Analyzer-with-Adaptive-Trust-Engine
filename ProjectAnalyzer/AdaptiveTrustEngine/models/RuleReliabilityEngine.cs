namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class RuleReliabilityEngine
{
    public double GetReliability(string ruleId)
    {
        return ruleId switch
        {
            "SQLI001" => 0.98,
            "PATH_TRAVERSAL_001" => 0.94,
            "HARD_CODED_CREDENTIALS_001" => 0.99,
            "XSS001" => 0.93,
            "COMMAND_INJECTION_001" => 0.97,

            _ => 0.80
        };
    }
}