using System.Text;

namespace ProjectAnalizer.Analyzer;

internal static class PromptBuilder
{
    public static string BuildPrompt(
        Dictionary<string, string> fileContents)
    {
        var builder = new StringBuilder();

        foreach (var (path, content) in fileContents)
        {
            builder.AppendLine($"----- {path}");
            builder.AppendLine(content);
            builder.AppendLine();
        }

        return builder.ToString();
    }
}