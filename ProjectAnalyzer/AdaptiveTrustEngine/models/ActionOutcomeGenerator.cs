using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class ActionOutcomeGenerator
{
    public CybersecurityEvent CreateEvent(
        EnhancedResult result)
    {
        return new CybersecurityEvent
        {
            EventType = "Static Vulnerability Analysis",
            Description =
                $"Analyzed {result.Category} vulnerability in {result.Path}"
        };
    }

    public SecurityAction CreateAction(
        CybersecurityEvent cybersecurityEvent,
        EnhancedResult result)
    {
        return new SecurityAction
        {
            EventId = cybersecurityEvent.EventId,
            ActionType = "Analyze Vulnerability",
            Description =
                $"Analyzed security finding {result.RuleId}",
            RiskScore =
                CalculateActionRisk(result)
        };
    }

    public ActionOutcome CreateOutcome(
        SecurityAction action,
        EnhancedResult result,
        ControlledActionResult control)
    {
        /*
         * This prototype does not execute real-world
         * cybersecurity commands.
         *
         * The outcome represents the result of the
         * controlled policy enforcement layer.
         */

        double outcomeScore =
            CalculateOutcomeScore(
                result,
                control);

        return new ActionOutcome
        {
            ActionId = action.ActionId,

            /*
             * The trust policy itself was successfully
             * applied to the proposed security action.
             */
            Success = true,

            OutcomeScore = outcomeScore,

            Description =
                $"Controlled action policy result: {control.Status}. " +
                $"{control.Description}"
        };
    }

    public Feedback CreateFeedback(
        SecurityAction action,
        ActionOutcome outcome)
    {
        return new Feedback
        {
            ActionId = action.ActionId,
            FeedbackScore = outcome.OutcomeScore,
            Source = "Controlled Action Outcome",
            Description =
                "Feedback generated from the controlled-action policy outcome."
        };
    }

    private double CalculateActionRisk(
        EnhancedResult result)
    {
        double risk = 0.50;

        if (result.Severity.Contains(
                "Critical",
                StringComparison.OrdinalIgnoreCase))
        {
            risk += 0.40;
        }
        else if (result.Severity.Contains(
                     "High",
                     StringComparison.OrdinalIgnoreCase))
        {
            risk += 0.25;
        }
        else if (result.Severity.Contains(
                     "Medium",
                     StringComparison.OrdinalIgnoreCase))
        {
            risk += 0.10;
        }

        return Math.Clamp(
            risk,
            0.0,
            1.0);
    }

    private double CalculateOutcomeScore(
        EnhancedResult result,
        ControlledActionResult control)
    {
        double score = 0.50;

        if (result.Evidence.Count >= 3)
        {
            score += 0.15;
        }

        if (result.RuleReliability >= 0.90)
        {
            score += 0.15;
        }
        else if (result.RuleReliability >= 0.75)
        {
            score += 0.10;
        }

        if (result.StartLine > 0 &&
            result.EndLine >= result.StartLine)
        {
            score += 0.10;
        }

        if (!string.IsNullOrWhiteSpace(
                result.KnowledgeRuleId) &&
            !result.KnowledgeRuleId.Equals(
                "GENERIC001",
                StringComparison.OrdinalIgnoreCase))
        {
            score += 0.10;
        }

        /*
         * Evidence that the controlled-action layer
         * produced a valid policy result.
         */
        if (!string.IsNullOrWhiteSpace(
                control.Status))
        {
            score += 0.05;
        }

        return Math.Clamp(
            score,
            0.0,
            1.0);
    }
}