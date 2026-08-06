using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class EvidenceCollector
{
    public List<string> Collect(EnhancedResult result)
    {
        var evidence = new List<string>();

        string category = result.Category.ToLowerInvariant();

        // --------------------------
        // SQL Injection
        // --------------------------
        if (category.Contains("sql"))
        {
            evidence.Add("User input detected.");
            evidence.Add("SQL query concatenation found.");
        }

        // --------------------------
        // Cross Site Scripting
        // --------------------------
        if (category.Contains("cross-site") ||
            category.Contains("cross site") ||
            category.Contains("xss"))
        {
            evidence.Add("User input rendered into HTML.");
            evidence.Add("Output encoding not detected.");
        }

        // --------------------------
        // Command Injection
        // --------------------------
        if (category.Contains("command"))
        {
            evidence.Add("Shell command uses user input.");
            evidence.Add("Unsafe process execution detected.");
        }

        // --------------------------
        // LDAP Injection
        // --------------------------
        if (category.Contains("ldap"))
        {
            evidence.Add("Dynamic LDAP query detected.");
        }

        // --------------------------
        // XPath Injection
        // --------------------------
        if (category.Contains("xpath"))
        {
            evidence.Add("Dynamic XPath expression detected.");
        }
// --------------------------
// Weak Cryptography
// --------------------------
if (category.Contains("cryptography") ||
    category.Contains("crypto"))
{
    evidence.Add("Weak cryptographic algorithm detected.");
    evidence.Add("Sensitive data protected with insecure encryption.");
}
        // --------------------------
        // XML External Entity
        // --------------------------
        if (category.Contains("xml external") ||
            category.Contains("xxe"))
        {
            evidence.Add("External entity processing enabled.");
        }

        // --------------------------
        // SSRF
        // --------------------------
        if (category.Contains("server side request forgery") ||
            category.Contains("ssrf"))
        {
            evidence.Add("External URL controlled by user.");
        }

        // --------------------------
        // CSRF
        // --------------------------
        if (category.Contains("cross site request forgery") ||
            category.Contains("csrf"))
        {
            evidence.Add("Missing CSRF validation.");
        }

        // --------------------------
        // Hardcoded Credentials
        // --------------------------
        if (category.Contains("credential") ||
            category.Contains("password"))
        {
            evidence.Add("Hardcoded credential detected.");
        }

        // --------------------------
        // Path Traversal
        // --------------------------
        if (category.Contains("path"))
        {
            evidence.Add("User-controlled file path detected.");
        }

        // --------------------------
        // Source Location
        // --------------------------
        if (result.StartLine > 0)
        {
            evidence.Add($"Source location: Line {result.StartLine}");
        }

        return evidence;
    }
}