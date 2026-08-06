namespace ProjectAnalizer.Analyzer;

internal static class FileLoader
{
    public static async Task<Dictionary<string, string>> LoadFilesAsync(
        string rootPath,
        IReadOnlyCollection<string> extensionsToAnalyze,
        IReadOnlyCollection<string> directoriesToSkip,
        CancellationToken cancellationToken = default)
    {
        var files = Directory
            .EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories)
            .Where(file =>
                extensionsToAnalyze.Any(ext =>
                    file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            .Where(file =>
                !directoriesToSkip.Any(dir =>
                    file.Contains(@$"{dir}\", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var fileContents = new Dictionary<string, string>();

        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(rootPath, file);

            fileContents[relativePath] =
                await File.ReadAllTextAsync(file, cancellationToken);
        }

        return fileContents;
    }
}