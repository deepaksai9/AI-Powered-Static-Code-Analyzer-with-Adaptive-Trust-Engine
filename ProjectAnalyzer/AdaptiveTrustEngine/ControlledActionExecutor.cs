using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class ControlledActionExecutor
{
    public ControlledActionResult Execute(
        SecurityAction action,
        ActionDecision decision)
    {
        return decision switch
        {
            ActionDecision.Allow =>
                new ControlledActionResult
                {
                    ActionId = action.ActionId,
                    Status = "EXECUTED",
                    Allowed = true,
                    VerificationRequired = false,
                    Description =
                        "Controlled security action permitted by trust policy."
                },

            ActionDecision.Verify =>
                new ControlledActionResult
                {
                    ActionId = action.ActionId,
                    Status = "VERIFICATION_REQUIRED",
                    Allowed = false,
                    VerificationRequired = true,
                    Description =
                        "Security action requires verification before execution."
                },

            ActionDecision.Restrict =>
                new ControlledActionResult
                {
                    ActionId = action.ActionId,
                    Status = "RESTRICTED",
                    Allowed = false,
                    VerificationRequired = false,
                    Description =
                        "Security action is restricted by the trust policy."
                },

            ActionDecision.Block =>
                new ControlledActionResult
                {
                    ActionId = action.ActionId,
                    Status = "BLOCKED",
                    Allowed = false,
                    VerificationRequired = false,
                    Description =
                        "Security action was denied by the trust policy."
                },

            _ =>
                new ControlledActionResult
                {
                    ActionId = action.ActionId,
                    Status = "BLOCKED",
                    Allowed = false,
                    VerificationRequired = false,
                    Description =
                        "Unknown decision. Action denied by default."
                }
        };
    }
}