namespace ProjectAnalyzer.AdaptiveTrustEngine.Matching;

internal static class RuleMatcher
{
    private static readonly Dictionary<string, string> Rules =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // ==========================
            // SQL Injection
            // ==========================
            ["sql"] = "SQL Injection",
            ["sqli"] = "SQL Injection",
            ["sql injection"] = "SQL Injection",
            ["database injection"] = "SQL Injection",
            ["improper sql"] = "SQL Injection",
            ["sql query"] = "SQL Injection",

            // ==========================
            // Cross Site Scripting
            // ==========================
            ["xss"] = "Cross-Site Scripting",
            ["cross site scripting"] = "Cross-Site Scripting",
            ["cross-site scripting"] = "Cross-Site Scripting",
            ["html injection"] = "Cross-Site Scripting",

            // ==========================
            // Path Traversal
            // ==========================
            ["path traversal"] = "Path Traversal",
            ["directory traversal"] = "Path Traversal",
            ["file traversal"] = "Path Traversal",
            ["path"] = "Path Traversal",

            // ==========================
            // Hardcoded Credentials
            // ==========================
            ["password"] = "Hardcoded Credentials",
            ["credential"] = "Hardcoded Credentials",
            ["secret"] = "Hardcoded Credentials",
            ["apikey"] = "Hardcoded Credentials",
            ["api key"] = "Hardcoded Credentials",
            ["token"] = "Hardcoded Credentials",

            // ==========================
            // Command Injection
            // ==========================
            ["command injection"] = "Command Injection",
            ["cmd injection"] = "Command Injection",
            ["shell injection"] = "Command Injection",

            // ==========================
            // LDAP Injection
            // ==========================
            ["ldap"] = "LDAP Injection",

            // ==========================
            // XPath Injection
            // ==========================
            ["xpath"] = "XPath Injection",

            // ==========================
            // XXE
            // ==========================
            ["xxe"] = "XML External Entity",
            ["xml external entity"] = "XML External Entity",

            // ==========================
            // SSRF
            // ==========================
            ["ssrf"] = "Server Side Request Forgery",
            ["server side request forgery"] = "Server Side Request Forgery",
            // ==========================
// Weak Cryptography
// ==========================
["cryptography"] = "Weak Cryptography",
["weak cryptography"] = "Weak Cryptography",
["weak encryption"] = "Weak Cryptography",
["md5"] = "Weak Cryptography",
["sha1"] = "Weak Cryptography",
["des"] = "Weak Cryptography",
["rc4"] = "Weak Cryptography",
["ecb"] = "Weak Cryptography",
["insecure hash"] = "Weak Cryptography",

            // ==========================
            // CSRF
            // ==========================
            ["csrf"] = "Cross Site Request Forgery",
            ["cross site request forgery"] = "Cross Site Request Forgery"
        };

    public static string Match(
        string ruleId,
        string description,
        string category,
        string message)
    {
        string search =
            $"{ruleId} {description} {category} {message}"
            .ToLowerInvariant();

        foreach (var rule in Rules)
        {
            if (search.Contains(rule.Key))
            {
                return rule.Value;
            }
        }

        return category;
    }
}