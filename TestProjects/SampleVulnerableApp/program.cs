using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        // Hardcoded Secret
        string password = "admin123";

        // SQL Injection
        string query = "SELECT * FROM Users WHERE Username='" + username + "'";

        Console.WriteLine(query);

        // Path Traversal
        Console.Write("Enter filename: ");
        string file = Console.ReadLine();

        string content = File.ReadAllText(file);

        Console.WriteLine(content);
    }
}