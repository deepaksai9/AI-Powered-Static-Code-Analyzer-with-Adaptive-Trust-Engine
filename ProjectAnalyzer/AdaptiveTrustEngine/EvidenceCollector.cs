using ProjectAnalyzer.AdaptiveTrustEngine.Models;

namespace ProjectAnalyzer.AdaptiveTrustEngine;

internal class EvidenceCollector
{
    public List<string> Collect(EnhancedResult result)
    {
        var evidence = new List<string>();

        /*
         * Do not rely only on Category.
         *
         * LLM output can contain inconsistent fields.
         * RuleDescription and Message often contain the
         * actual vulnerability type, so they are also used.
         */
        string text =
            $"{result.RuleId} " +
            $"{result.RuleDescription} " +
            $"{result.Message} " +
            $"{result.Category}"
            .ToLowerInvariant();

        /*
         * SQL Injection
         */
        if (text.Contains("sql injection") ||
            text.Contains("sqli") ||
            text.Contains("sql query") ||
            text.Contains("sql command") ||
            text.Contains("sql concatenation"))
        {
            evidence.Add("User input detected.");
            evidence.Add("SQL query concatenation found.");
        }

        /*
         * Cross-Site Scripting
         */
        if (text.Contains("cross-site scripting") ||
            text.Contains("cross site scripting") ||
            text.Contains("xss") ||
            text.Contains("html injection"))
        {
            evidence.Add("User input rendered into HTML.");
            evidence.Add("Output encoding not detected.");
        }

        /*
         * Command Injection
         */
        if (text.Contains("command injection") ||
            text.Contains("cmd injection") ||
            text.Contains("shell command") ||
            text.Contains("command execution") ||
            text.Contains("process execution"))
        {
            evidence.Add("Shell command uses user input.");
            evidence.Add("Unsafe process execution detected.");
        }

        /*
         * LDAP Injection
         */
        if (text.Contains("ldap injection") ||
            text.Contains("ldap query"))
        {
            evidence.Add("Dynamic LDAP query detected.");
        }

        /*
         * XPath Injection
         */
        if (text.Contains("xpath injection") ||
            text.Contains("xpath query"))
        {
            evidence.Add("Dynamic XPath expression detected.");
        }

        /*
         * XML External Entity
         */
        if (text.Contains("xml external entity") ||
            text.Contains("external entity") ||
            text.Contains("xxe"))
        {
            evidence.Add("External entity processing enabled.");
        }

        /*
         * Server-Side Request Forgery
         */
        if (text.Contains("server-side request forgery") ||
            text.Contains("server side request forgery") ||
            text.Contains("ssrf"))
        {
            evidence.Add("External URL controlled by user.");
        }

        /*
         * Cross-Site Request Forgery
         */
        if (text.Contains("cross-site request forgery") ||
            text.Contains("cross site request forgery") ||
            text.Contains("csrf"))
        {
            evidence.Add("Missing CSRF validation.");
        }

        /*
         * Hardcoded Credentials
         */
        if (text.Contains("hardcoded credential") ||
            text.Contains("hardcoded credentials") ||
            text.Contains("hardcoded password") ||
            text.Contains("hardcoded secret") ||
            text.Contains("hardcoded api key") ||
            text.Contains("credentials are hardcoded") ||
            text.Contains("api key is hardcoded"))
        {
            evidence.Add("Hardcoded credential detected.");
        }

        /*
         * Path Traversal
         */
        if (text.Contains("path traversal") ||
            text.Contains("directory traversal") ||
            text.Contains("user-controlled file path") ||
            text.Contains("file path") &&
            text.Contains("user input"))
        {
            evidence.Add("User-controlled file path detected.");
        }

        /*
         * Weak Cryptography
         *
         * IMPORTANT:
         * This is deliberately checked using cryptography
         * terminology rather than simply Category.
         *
         * Therefore a finding described as SQL Injection but
         * incorrectly categorized as Cryptography will NOT
         * receive cryptography evidence.
         */
        if (text.Contains("weak cryptography") ||
            text.Contains("weak cryptographic") ||
            text.Contains("weak encryption") ||
            text.Contains("insecure encryption") ||
            text.Contains("des algorithm") ||
            text.Contains("des encryption") ||
            text.Contains("cryptographic failure"))
        {
            evidence.Add("Weak cryptographic algorithm detected.");
            evidence.Add("Sensitive data protected with insecure encryption.");
        }

        /*
         * Insecure Deserialization
         */
        if (text.Contains("insecure deserialization") ||
            text.Contains("unsafe deserialization") ||
            text.Contains("deserializing untrusted") ||
            text.Contains("untrusted data") &&
            text.Contains("deserial"))
        {
            evidence.Add("Untrusted data deserialization detected.");
            evidence.Add("Deserialization performed without adequate validation.");
        }

        /*
         * Authentication
         */
        if (text.Contains("broken authentication") ||
            text.Contains("weak authentication") ||
            text.Contains("authentication failure") ||
            text.Contains("jwt authentication") ||
            text.Contains("jwt token"))
        {
            evidence.Add("Authentication mechanism requires review.");
        }

        /*
         * Authorization
         */
        if (text.Contains("broken authorization") ||
            text.Contains("authorization check") ||
            text.Contains("access control") ||
            text.Contains("unauthorized access"))
        {
            evidence.Add("Authorization or access-control validation requires review.");
        }

        /*
         * Always preserve source-location evidence.
         */
        if (result.StartLine > 0)
        {
            evidence.Add(
                $"Source location: Line {result.StartLine}");
        }

        return evidence;
    }
}