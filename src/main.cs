using System.Net;
using System.Net.Sockets;
// You can use print statements as follows for debugging, they'll be visible when running tests.
// Console.WriteLine("Logs from your program will appear here!");

// Uncomment this line to pass the first stage
Console.Write("$ ");

// Wait for user input
string? input = Console.ReadLine();

string[] inputArr = input.Split();

Dictionary<string, Command> dict = new Dictionary<string, Command>();

dict["echo"] = Echo;

while (input != "exit 0")
{
    if (input!=null && !dict.ContainsKey(inputArr[0]))
    {
        Console.WriteLine($"{input}: not found");
    }
    else
    {
        Console.WriteLine(dict[inputArr[0]](inputArr));
    }
    Console.Write("$ ");
    input = Console.ReadLine();
    inputArr = input.Split();
}



string Echo(string[] s){

    string result ="";
    for(int i = 1; i<s.Length; i++) 
    { 
        result+= s[i];
        if(i!=s.Length-1) result+=" ";
    }
    return result;
}

delegate string Command(string[] s);