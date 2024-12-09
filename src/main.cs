using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

//var x = "../../../".RemoveDotSlash();

IDictionary data = Environment.GetEnvironmentVariables();

var PATH_DICT = data["PATH"];
var HOME_DICT = data["HOME"];
string[]? paths = PATH_DICT.ToString().Split(':');

string[]? home = HOME_DICT.ToString().Split('/');

string? input;
string cwd = System.IO.Directory.GetCurrentDirectory();
string[] inputArr;

Dictionary<string, Command> dict = new Dictionary<string, Command>();

dict["echo"] = echo;
dict["type"] = type;
dict["exit"] = exit;
dict["pwd"] = pwd;
dict["cd"] = cd;
dict["cat"] = cat;

while (true)
{
    Console.Write("$ ");
    input = Console.ReadLine();
    inputArr = input.SplitParam();

    if (input != null && input != "" && !dict.ContainsKey(inputArr[0]))
    {
        try
        {
            Process notePad = new Process();
            notePad.StartInfo.FileName = inputArr[0];
            notePad.StartInfo.Arguments = inputArr.Concatinate(1, inputArr.Length);
            notePad.Start();
            Thread.Sleep(10);
        }
        catch (Exception)
        {
            Console.WriteLine($"{input}: not found");
        }

        continue;
    }
    else if (inputArr[0] == "exit")
        break;
    else
    {
        string log = dict[inputArr[0]](inputArr);
        if (log.Length > 0)
            Console.WriteLine(log);
    }
}

string cat(string[] s)
{
    List<string> dir_list = new List<string>();
    string result = "";

    for (int i = 1; i < s.Length; i++) dir_list.Add(s[i]);

    foreach (string dir in dir_list)
    {
        if (dir != "")
        {
            try
            {
                string contents = File.ReadAllText(@dir);
                result += contents;
                if (result.EndsWith('\n')) result = result.Remove(result.Length - 1, 1);
                if (result.EndsWith("\r\n")) result = result.Remove(result.Length - 1, 2);
            }
            catch (Exception)

            {
                try
                {
                    string contents = File.ReadAllText(@cwd + "/" + @dir);
                    result += contents;
                    if (result.EndsWith('\n')) result = result.Remove(result.Length - 1, 1);
                    if (result.EndsWith("\r\n")) result = result.Remove(result.Length - 1, 2);
                }
                catch (Exception) { return $"{dir}: not found"; }

            }

        }

    }

    return result;
}

string pwd(string[] s)
{
    return cwd;
}

string cd(string[] s)
{
    string[] dirArr = s[1].Split('/');
    string[] cwdArr = cwd.Split('/');
    LinkedList<string> dirDq = new LinkedList<string>();

    // usr/local/bin/lol/../../


    if (dirArr[0] is ".." or ".")
    {
        foreach (string p in cwdArr)
        {
            if (p == ".." && dirDq.Count > 0)
                dirDq.RemoveLast();
            else if (p == ".")
                continue;
            else if (p != "")
                dirDq.AddLast(p);
        }
    }

    foreach (string p in dirArr)
    {
        if (p == ".." && dirDq.Count > 0)
            dirDq.RemoveLast();
        else if (p == ".")
            continue;
        else if (p == "~")
        {
            foreach (string x in home)
                if (x != "")
                    dirDq.AddLast(x);
        }
        else if (p != "")
            dirDq.AddLast(p);
    }

    string fixedDir = "";

    while (dirDq.Count > 0)
    {
        fixedDir += "/" + dirDq.First.Value;
        dirDq.RemoveFirst();
    }

    if (Directory.Exists(cwd + "/" + fixedDir))
        cwd += "/" + s[1];
    else if (Directory.Exists(fixedDir))
        cwd = fixedDir;
    else
        return $"cd: {s[1]}: No such file or directory";

    return "";
}

string exit(string[] s)
{
    return "";
}

string type(string[] s)
{
    string result;
    string command = s[1];
    if (s[1] == "cat")
    {
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

    }

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
        if (s[i] != "")
        {
            result += s[i];
            if (i != s.Length - 1)
                result += " ";
        }
    }
    if (result[0] == '\'') { result = result.Remove(0, 1); result = result.Remove(result.Length - 1, 1); }
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
            if (i != end - 1)
                result += " ";
        }

        return result;
    }

    public static string[] SplitParam(this string str)
    {
        List<string> ans = new List<string>();
        bool literal = false;
        string temp = "";
        foreach (var c in str)
        {
            if (!literal)
            {
                if (c == '\'') literal = true;
                else if (c != ' ') temp += c;
                else { ans.Add(temp); temp = ""; }
            }
            else
            {

                if (c != '\'') temp += c;
                else { ans.Add(temp); literal = false; temp = ""; }
            }
        }
        if (temp is not "") ans.Add(temp);
        return ans.ToArray();
    }

}

delegate string Command(string[] s);