using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ProjectAnalizer;

internal static partial class Constants
{
    internal const string SystemPrompt = """
You are an expert Static Application Security Testing (SAST) engine.

Analyze ONLY the provided source code.

Report ONLY real security vulnerabilities.

Ignore:
- Code smells
- Performance issues
- Formatting issues
- Best practice suggestions

Detect only:

- SQL Injection
- Cross-Site Scripting
- Command Injection
- Path Traversal
- Hardcoded Credentials
- Insecure Deserialization
- Broken Authentication
- Broken Authorization
- Weak Cryptography
- Insecure File Handling

Return ONLY a JSON array.

Each object MUST contain:

RuleId
RuleDescription
Level
Message
Path
Category
StartLine
EndLine
StartColumn
EndColumn

If StartColumn or EndColumn cannot be determined,
set them to 1.

Allowed Category values:

SQL Injection
Cross-Site Scripting
Command Injection
Path Traversal
Hardcoded Credentials
Insecure Deserialization
Authentication
Authorization
Cryptography
File Handling

Do not return explanations.

Do not return markdown.

Return valid JSON only.
""";
    internal const string SarifSchema = "https://json.schemastore.org/sarif-2.1.0.json";

    internal const string SarifVersion = "2.1.0";

    internal static readonly JsonSerializerOptions ChatJsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    internal static readonly JsonSerializerOptions SarifJsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    [GeneratedRegex(@"```json\s*(.*?)\s*```", RegexOptions.Singleline)]
    internal static partial Regex JsonRegex();
}