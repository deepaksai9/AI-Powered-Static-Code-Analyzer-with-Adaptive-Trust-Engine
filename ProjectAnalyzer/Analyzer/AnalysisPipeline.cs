using ProjectAnalyzer.Dtos;

namespace ProjectAnalizer.Analyzer;

internal class AnalysisPipeline
{
    private readonly HttpClient _httpClient;

    public AnalysisPipeline(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LlmResponseDto?> AnalyzeProjectAsync(
        string model,
        string rootPath,
        IReadOnlyCollection<string> extensionsToAnalyze,
        IReadOnlyCollection<string> directoriesToSkip,
        CancellationToken cancellationToken = default)
    {
        // Step 1 - Load project files
        var fileContents = await FileLoader.LoadFilesAsync(
            rootPath,
            extensionsToAnalyze,
            directoriesToSkip,
            cancellationToken);

        // Step 2 - Build prompt
        var prompt = PromptBuilder.BuildPrompt(fileContents);

        // Step 3 - Call Ollama
        var ollama = new OllamaClient(_httpClient);

        return await ollama.AnalyzeAsync(
            model,
            prompt,
            cancellationToken);
    }
}