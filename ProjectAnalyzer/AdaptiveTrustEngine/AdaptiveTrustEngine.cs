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
    private readonly ControlledActionExecutor _controlledActionExecutor = new();

    private readonly TrustScoreCalculator _trustScoreCalculator = new();

    private readonly RecommendationEngine _recommendationEngine = new();

    private readonly DynamicTrustManager _dynamicTrustManager = new();

    private readonly TrustEvidenceCalculator _trustEvidenceCalculator = new();

    private readonly ActionRiskEvaluator _actionRiskEvaluator = new();

    private readonly ActionOutcomeGenerator _actionOutcomeGenerator = new();


    public List<EnhancedResult> Process(
        IEnumerable<LlmResponseDto.ResultDto> llmResults)
    {
        llmResults =
            _normalizer.Normalize(llmResults);

        var enhancedResults =
            new List<EnhancedResult>();


        foreach (var result in llmResults)
        {
            // ============================================
            // 1. Create Enhanced Result
            // ============================================

            var enhanced = new EnhancedResult
            {
                RuleId = result.RuleId,

                RuleDescription =
                    result.RuleDescription,

                Message =
                    result.Message,

                Path =
                    result.Path,

                Category =
                    result.Category,

                StartLine =
                    result.StartLine,

                EndLine =
                    result.EndLine,

                IsVerified = true
            };


            // ============================================
            // 2. Evidence Collection
            // ============================================

            enhanced.Evidence =
                _evidenceCollector.Collect(
                    enhanced);


            // ============================================
            // 3. Vulnerability Knowledge
            // ============================================

            VulnerabilityInfo info =
                _recommendationEngine
                    .GetKnowledge(enhanced);


            enhanced.KnowledgeRuleId =
                info.RuleId;

            enhanced.Title =
                info.Title;

            enhanced.Risk =
                info.Risk;

            enhanced.Severity =
                info.Severity;

            enhanced.CWE =
                info.CWE;

            enhanced.Owasp =
                info.Owasp;

            enhanced.Priority =
                info.Priority;

            enhanced.EstimatedFixMinutes =
                info.EstimatedFixMinutes;

            enhanced.RuleReliability =
                info.RuleReliability;

            enhanced.Recommendation =
                info.Recommendation;

            enhanced.ExampleFix =
                info.ExampleFix;

            enhanced.Reference =
                info.Reference;


            // ============================================
            // 4. Confidence Calculation
            // ============================================

            enhanced.ConfidenceScore =
                _confidenceCalculator.Calculate(
                    enhanced);


            // ============================================
            // 5. Create Cybersecurity Event
            // ============================================

            CybersecurityEvent cybersecurityEvent =
                _actionOutcomeGenerator.CreateEvent(
                    enhanced);


            // ============================================
            // 6. Create Security Action
            // ============================================

            SecurityAction securityAction =
                _actionOutcomeGenerator.CreateAction(
                    cybersecurityEvent,
                    enhanced);


            enhanced.ActionRiskScore =
                securityAction.RiskScore;


            // ============================================
            // 7. Evaluate Security Action
            // ============================================

            // Use the preliminary trust value based on
            // confidence and initial evidence.
            //
            // Action decision is evaluated before the
            // final action outcome is generated.

            _trustEvidenceCalculator.Calculate(
                enhanced);

            enhanced.TrustScore =
                _trustScoreCalculator.Calculate(
                    enhanced);

            ActionDecision decision =
    _actionRiskEvaluator.Evaluate(enhanced);

enhanced.ActionDecision =
    decision.ToString();

ControlledActionResult controlledAction =
    _controlledActionExecutor.Execute(
        securityAction,
        decision);

enhanced.ControlStatus =
    controlledAction.Status;

enhanced.ActionAllowed =
    controlledAction.Allowed;

enhanced.VerificationRequired =
    controlledAction.VerificationRequired;

enhanced.ControlDescription =
    controlledAction.Description;

            // ============================================
            // 8. Create Action Outcome
            // ============================================

            ActionOutcome actionOutcome =
    _actionOutcomeGenerator.CreateOutcome(
        securityAction,
        enhanced,
        controlledAction);

            enhanced.ActionSucceeded =
                actionOutcome.Success;

            enhanced.OutcomeScore =
                actionOutcome.OutcomeScore;


            // ============================================
            // 9. Generate Feedback
            // ============================================

            Feedback feedback =
                _actionOutcomeGenerator.CreateFeedback(
                    securityAction,
                    actionOutcome);


            enhanced.FeedbackScore =
                feedback.FeedbackScore;

            enhanced.FeedbackSource =
                feedback.Source;


            // ============================================
            // 10. Recalculate Trust Evidence
            //     using actual action outcome
            // ============================================

            _trustEvidenceCalculator.Calculate(
                enhanced);


            // ============================================
            // 11. Recalculate Current Trust
            // ============================================

            enhanced.TrustScore =
                _trustScoreCalculator.Calculate(
                    enhanced);


            // ============================================
            // 12. Dynamic Trust Update
            // ============================================

            _dynamicTrustManager.ApplyToResult(
                enhanced,
                feedback.FeedbackScore,
                actionOutcome.Success);


            // ============================================
            // 13. Store Result
            // ============================================

            enhancedResults.Add(
                enhanced);
        }


        return enhancedResults;
    }
}