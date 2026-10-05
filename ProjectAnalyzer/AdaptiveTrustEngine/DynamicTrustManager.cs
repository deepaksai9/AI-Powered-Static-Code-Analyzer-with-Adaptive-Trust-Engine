using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class DynamicTrustManager
{
    private readonly AgentTrustState _agentState;

    private readonly TrustStateStore _stateStore;

    public DynamicTrustManager()
    {
        _stateStore = new TrustStateStore();

        _agentState = _stateStore.Load();
    }

    public AgentTrustState GetState()
    {
        return _agentState;
    }

    public double GetPreviousTrust()
    {
        return _agentState.TrustScore;
    }

    public int GetEvaluationCount()
    {
        return _agentState.EvaluationCount;
    }

    public double UpdateTrust(
        double currentTrust,
        double feedbackScore,
        bool actionSucceeded)
    {
        double previousTrust =
            _agentState.TrustScore;

        /*
         * Historical trust has the largest influence.
         * Current evidence and feedback provide adaptation.
         */
        double updatedTrust =
            (0.60 * previousTrust) +
            (0.25 * currentTrust) +
            (0.15 * feedbackScore);

        /*
         * Failed security actions reduce trust.
         */
        if (!actionSucceeded)
        {
            updatedTrust -= 0.10;

            _agentState.FailedActions++;
        }
        else
        {
            _agentState.SuccessfulActions++;
        }

        updatedTrust =
            Math.Clamp(
                updatedTrust,
                0.0,
                1.0);

        _agentState.TrustScore =
            updatedTrust;

        _agentState.EvaluationCount++;

        /*
         * Running average feedback.
         */
        _agentState.AverageFeedback =
            ((_agentState.AverageFeedback *
              (_agentState.EvaluationCount - 1))
             + feedbackScore)
            / _agentState.EvaluationCount;

        _agentState.LastUpdated =
            DateTime.UtcNow;

        /*
         * Persist the updated agent trust state.
         */
        _stateStore.Save(_agentState);

        return updatedTrust;
    }

    public void ApplyToResult(
        EnhancedResult result,
        double feedbackScore,
        bool actionSucceeded)
    {
        double previousTrust =
            _agentState.TrustScore;

        int evaluationCount =
            _agentState.EvaluationCount;

        result.PreviousTrustScore =
            previousTrust;

        result.EvaluationCount =
            evaluationCount;

        result.HasHistoricalTrust =
            evaluationCount > 0;

        result.UpdatedTrustScore =
            UpdateTrust(
                result.TrustScore,
                feedbackScore,
                actionSucceeded);

        result.TrustScore =
            result.UpdatedTrustScore;
    }
}