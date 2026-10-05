using ProjectAnalyzer.Dtos;

namespace ProjectAnalyzer.AdaptiveTrustEngine.Normalizer;

internal class LlmOutputNormalizer
{
    public IEnumerable<LlmResponseDto.ResultDto> Normalize(
        IEnumerable<LlmResponseDto.ResultDto> results)
    {
        foreach (var result in results)
        {
            NormalizeSeverity(result);
            NormalizeCategory(result);
            NormalizeRuleId(result);
            NormalizePath(result);
        }

        return results;
    }

    private static void NormalizeSeverity(
        LlmResponseDto.ResultDto result)
    {
        result.Level = result.Level switch
        {
            SarifDto.SeverityLevel.Error =>
                SarifDto.SeverityLevel.Error,

            SarifDto.SeverityLevel.Warning =>
                SarifDto.SeverityLevel.Warning,

            SarifDto.SeverityLevel.Note =>
                SarifDto.SeverityLevel.Note,

            _ =>
                SarifDto.SeverityLevel.Warning
        };
    }

    private static void NormalizeCategory(
        LlmResponseDto.ResultDto result)
    {
        /*
         * Use all available textual evidence.
         *
         * The LLM may provide an incorrect Category while
         * RuleDescription or Message contains the correct
         * vulnerability type.
         */
        string text =
            $"{result.RuleDescription} " +
            $"{result.Message} " +
            $"{result.Category}"
            .Trim()
            .ToLowerInvariant();

        /*
         * Order matters.
         * More specific vulnerability patterns are checked first.
         */

        // SQL Injection
        if (text.Contains("sql injection") ||
            text.Contains("sqli") ||
            text.Contains("sql query") ||
            text.Contains("sql command") ||
            text.Contains("sql concatenation"))
        {
            result.Category = "SQL Injection";
            return;
        }

        // Cross-Site Scripting
        if (text.Contains("cross-site scripting") ||
            text.Contains("cross site scripting") ||
            text.Contains("xss") ||
            text.Contains("html injection") ||
            text.Contains("javascript injection"))
        {
            result.Category = "Cross-Site Scripting";
            return;
        }

        // Command Injection
        if (text.Contains("command injection") ||
            text.Contains("cmd injection") ||
            text.Contains("shell command") ||
            text.Contains("command execution") ||
            text.Contains("process execution"))
        {
            result.Category = "Command Injection";
            return;
        }

        // Path Traversal
        if (text.Contains("path traversal") ||
            text.Contains("directory traversal") ||
            text.Contains("../") ||
            text.Contains("user-controlled file path") ||
            text.Contains("file path") &&
            text.Contains("user input"))
        {
            result.Category = "Path Traversal";
            return;
        }

        // Hardcoded Credentials
        if (text.Contains("hardcoded credential") ||
            text.Contains("hardcoded credentials") ||
            text.Contains("hardcoded password") ||
            text.Contains("hardcoded api key") ||
            text.Contains("hardcoded secret") ||
            text.Contains("api key is hardcoded") ||
            text.Contains("credentials are hardcoded"))
        {
            result.Category = "Hardcoded Credentials";
            return;
        }

        // Weak Cryptography
        if (text.Contains("weak cryptography") ||
            text.Contains("weak cryptographic") ||
            text.Contains("weak encryption") ||
            text.Contains("insecure encryption") ||
            text.Contains("des algorithm") ||
            text.Contains("des encryption") ||
            text.Contains("cryptographic failure"))
        {
            result.Category = "Cryptography";
            return;
        }

        // LDAP Injection
        if (text.Contains("ldap injection") ||
            text.Contains("ldap query"))
        {
            result.Category = "LDAP Injection";
            return;
        }

        // XPath Injection
        if (text.Contains("xpath injection") ||
            text.Contains("xpath query"))
        {
            result.Category = "XPath Injection";
            return;
        }

        // XML External Entity
        if (text.Contains("xml external entity") ||
            text.Contains("xxe") ||
            text.Contains("external entity"))
        {
            result.Category = "XML External Entity";
            return;
        }

        // Server-Side Request Forgery
        if (text.Contains("server-side request forgery") ||
            text.Contains("server side request forgery") ||
            text.Contains("ssrf"))
        {
            result.Category = "Server Side Request Forgery";
            return;
        }

        // Cross-Site Request Forgery
        if (text.Contains("cross-site request forgery") ||
            text.Contains("cross site request forgery") ||
            text.Contains("csrf"))
        {
            result.Category = "Cross Site Request Forgery";
            return;
        }

        // Insecure Deserialization
        if (text.Contains("insecure deserialization") ||
            text.Contains("unsafe deserialization") ||
            text.Contains("deserializing untrusted"))
        {
            result.Category = "Insecure Deserialization";
            return;
        }

        // Authorization
        if (text.Contains("broken authorization") ||
            text.Contains("authorization check") ||
            text.Contains("unauthorized access") ||
            text.Contains("access control"))
        {
            result.Category = "Authorization";
            return;
        }

        // Authentication
        if (text.Contains("broken authentication") ||
            text.Contains("weak authentication") ||
            text.Contains("authentication failure") ||
            text.Contains("jwt token") ||
            text.Contains("token generation"))
        {
            result.Category = "Authentication";
            return;
        }

        /*
         * Preserve the original category when no specific
         * normalization rule matches.
         */
        result.Category = NormalizeExistingCategory(
            result.Category);
    }

    private static string NormalizeExistingCategory(
        string category)
    {
        string value =
            category.Trim().ToLowerInvariant();

        return value switch
        {
            "sql injection" =>
                "SQL Injection",

            "cross-site scripting" =>
                "Cross-Site Scripting",

            "xss" =>
                "Cross-Site Scripting",

            "command injection" =>
                "Command Injection",

            "path traversal" =>
                "Path Traversal",

            "directory traversal" =>
                "Path Traversal",

            "hardcoded credentials" =>
                "Hardcoded Credentials",

            "cryptography" =>
                "Cryptography",

            "weak cryptography" =>
                "Cryptography",

            "insecure deserialization" =>
                "Insecure Deserialization",

            "authentication" =>
                "Authentication",

            "authorization" =>
                "Authorization",

            "ldap injection" =>
                "LDAP Injection",

            "xpath injection" =>
                "XPath Injection",

            "xml external entity" =>
                "XML External Entity",

            "ssrf" =>
                "Server Side Request Forgery",

            "server side request forgery" =>
                "Server Side Request Forgery",

            "csrf" =>
                "Cross Site Request Forgery",

            "cross site request forgery" =>
                "Cross Site Request Forgery",

            _ =>
                category
        };
    }

    private static void NormalizeRuleId(
    LlmResponseDto.ResultDto result)
{
    result.RuleId = result.Category switch
    {
        "SQL Injection" =>
            "SQLI001",

        "Hardcoded Credentials" =>
            "HCRED001",

        "Path Traversal" =>
            "PATH001",

        "Cross-Site Scripting" =>
            "XSS001",

        "Command Injection" =>
            "CMDI001",

        "Cryptography" =>
            "CRYPTO001",

        "LDAP Injection" =>
            "LDAP001",

        "XPath Injection" =>
            "XPATH001",

        "Insecure Deserialization" =>
            "DESER001",

        _ =>
            result.RuleId
    };
}

    private static void NormalizePath(
        LlmResponseDto.ResultDto result)
    {
        result.Path =
            result.Path
                .Replace("\\", "/")
                .Trim();
    }
}