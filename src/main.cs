using System.Net;
using System.Net.Sockets;
using System;
using System.Collections;
using System.Diagnostics;




IDictionary data = Environment.GetEnvironmentVariables();

var path = data["PATH"];
string[] paths = path.ToString().Split(':');


string? input;

string[] inputArr;

Dictionary<string, Command> dict = new Dictionary<string, Command>();

dict["echo"] = echo;
dict["type"] = type;
dict["exit"] = exit;

while (true)
{

    Console.Write("$ ");
    input = Console.ReadLine();
    inputArr = input.Split();

    if (input != null && !dict.ContainsKey(inputArr[0]))

    {
        try
        {
            Process notePad = new Process();
            notePad.StartInfo.FileName = inputArr[0];
            notePad.StartInfo.Arguments = inputArr.Concatinate(1, inputArr.Length);
            notePad.Start();
            
        }

        catch (Exception)
        {
            Console.WriteLine($"{input}: not found");
        }
        continue;
    
    }
    else if (inputArr[0] == "exit")
    {
        //Console.WriteLine(dict[inputArr[0]](inputArr));
        break;
    }
    else
        Console.WriteLine(dict[inputArr[0]](inputArr));


}





string exit(string[] s)
{
    return "";
}

string type(string[] s)
{
    string result;
    string command = s[1];

    if (dict.ContainsKey(s[1]))
    {
        result = $"{s[1]} is a shell builtin";
        return result;
    }

    foreach (string p in paths)
    {
        string[] cwdFiles;
        try
        {
            cwdFiles = Directory.GetFiles(p);
        }
        catch (Exception)
        {
            continue;
        }

        foreach (string f in cwdFiles)
        {
            if (f.EndsWith("/" + command))
            {
                result = $"{command} is {f}";
                return result;
            }
        }
    }



    result = $"{s[1]}: not found";



    return result;
}

string echo(string[] s)
{

    string result = "";
    for (int i = 1; i < s.Length; i++)
    {
        result += s[i];
        if (i != s.Length - 1) result += " ";
    }
    return result;
}


public static class StringExtensions
{
    public static string Concatinate(this string[] str, int start, int end)
    {
        string result = "";
        for (int i = start; i < end; i++)

        {
            result += str[i];
            if (i != end - 1) result += " ";
        }


        return result;
    }
}
delegate string Command(string[] s);