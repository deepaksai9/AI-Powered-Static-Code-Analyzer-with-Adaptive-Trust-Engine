using System.Text.Json;
using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class TrustStateStore
{
    private readonly string _filePath;

    public TrustStateStore(string filePath = "trust_state.json")
    {
        _filePath = filePath;
    }

    public AgentTrustState Load()
    {
        if (!File.Exists(_filePath))
        {
            return new AgentTrustState();
        }

        try
        {
            string json =
                File.ReadAllText(_filePath);

            var state =
                JsonSerializer.Deserialize<AgentTrustState>(json);

            return state ?? new AgentTrustState();
        }
        catch
        {
            return new AgentTrustState();
        }
    }

    public void Save(AgentTrustState state)
    {
        var options =
            new JsonSerializerOptions
            {
                WriteIndented = true
            };

        string json =
            JsonSerializer.Serialize(
                state,
                options);

        File.WriteAllText(
            _filePath,
            json);
    }
}