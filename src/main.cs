using System.Net;
using System.Net.Sockets;
// You can use print statements as follows for debugging, they'll be visible when running tests.
// Console.WriteLine("Logs from your program will appear here!");

// Uncomment this line to pass the first stage
Console.Write("$ ");

// Wait for user input
string? input = Console.ReadLine();


Dictionary<string, int> dict = new Dictionary<string, int>();

while (true)
{
    if (!dict.ContainsKey(input))
    {
        Console.WriteLine($"{input}: not found");
    }
    Console.Write("$ ");
    input = Console.ReadLine();
}
