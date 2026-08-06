using ProjectAnalyzer.AdaptiveTrustEngine.Models;
using ProjectAnalyzer.AdaptiveTrustEngine.KnowledgeBase;
using ProjectAnalyzer.Dtos;
using ProjectAnalyzer.AdaptiveTrustEngine.Normalizer;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class AdaptiveTrustEngine
{
    private readonly ConfidenceCalculator _confidenceCalculator = new();
    private readonly LlmOutputNormalizer _normalizer = new();
    private readonly EvidenceCollector _evidenceCollector = new();
    private readonly TrustScoreCalculator _trustScoreCalculator = new();
    private readonly RecommendationEngine _recommendationEngine = new();

    public List<EnhancedResult> Process(IEnumerable<LlmResponseDto.ResultDto> llmResults)
    {
        llmResults = _normalizer.Normalize(llmResults);

        var enhancedResults = new List<EnhancedResult>();

        foreach (var result in llmResults)
        {
            var enhanced = new EnhancedResult
            {
                RuleId = result.RuleId,
                RuleDescription = result.RuleDescription,
                Message = result.Message,
                Path = result.Path,
                Category = result.Category,
                StartLine = result.StartLine,
                EndLine = result.EndLine,

                // Temporary verification logic
                IsVerified = true
            };

            // Collect evidence
            enhanced.Evidence = _evidenceCollector.Collect(enhanced);

            // Get vulnerability information
            VulnerabilityInfo info = _recommendationEngine.GetKnowledge(enhanced);

            enhanced.KnowledgeRuleId = info.RuleId;
            enhanced.Title = info.Title;
            enhanced.Risk = info.Risk;
            enhanced.Severity = info.Severity;
            enhanced.CWE = info.CWE;
            enhanced.Owasp = info.Owasp;
            enhanced.Priority = info.Priority;
            enhanced.EstimatedFixMinutes = info.EstimatedFixMinutes;
            enhanced.RuleReliability = info.RuleReliability;
            enhanced.Recommendation = info.Recommendation;
            enhanced.ExampleFix = info.ExampleFix;
            enhanced.Reference = info.Reference;

            // Calculate confidence
            enhanced.ConfidenceScore =
                _confidenceCalculator.Calculate(enhanced);

            // Calculate trust
            enhanced.TrustScore =
                _trustScoreCalculator.Calculate(enhanced);

            enhancedResults.Add(enhanced);
        }

        return enhancedResults;
    }
}