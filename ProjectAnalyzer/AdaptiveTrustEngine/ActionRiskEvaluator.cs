using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class ActionRiskEvaluator
{
    public ActionDecision Evaluate(EnhancedResult result)
    {
        double trust = result.TrustScore;
        double risk = result.ActionRiskScore;

        /*
         * Risk-aware decision policy.
         *
         * Trust represents confidence in the agent's
         * current behavior and evidence.
         *
         * ActionRiskScore represents the sensitivity
         * of the proposed cybersecurity action.
         *
         * Thresholds are prototype design choices and
         * are not empirically validated yet.
         */

        // Very low trust: do not permit the action.
        if (trust < 0.30)
        {
            return ActionDecision.Block;
        }

        // Low trust: restrict the action.
        if (trust < 0.60)
        {
            return ActionDecision.Restrict;
        }

        /*
         * Highly sensitive actions require stronger trust.
         * A critical/high-risk action with trust below 80%
         * is restricted.
         */
        if (risk >= 0.90 && trust < 0.80)
        {
            return ActionDecision.Restrict;
        }

        /*
         * High-risk actions require human/secondary verification.
         */
        if (risk >= 0.75)
        {
            return ActionDecision.Verify;
        }

        /*
         * Moderate trust also requires verification.
         */
        if (trust < 0.85)
        {
            return ActionDecision.Verify;
        }

        /*
         * High enough trust with moderate/low action risk.
         */
        return ActionDecision.Allow;
    }
}