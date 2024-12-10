using System.Diagnostics;
using sys;
using commands;
using StringExtensions;

string input;
string[] inputArr;

// test
while (true)
{
    Console.Write("$ ");
    input = Console.ReadLine()!;

    if (input.Length == 0) continue;

    inputArr = input.SplitParam();
    string command = inputArr[0];

    if (!Commands.ContainsCommand(command))
    {
        try
        {
            Process notePad = new Process();
            notePad.StartInfo.FileName = inputArr[0];
            notePad.StartInfo.Arguments = inputArr.Concatinate(1, inputArr.Length);
            notePad.Start();
            Thread.Sleep(1);
        }
        catch (Exception)
        {
            Console.WriteLine($"{input}: not found");
        }

        continue;
    }
    else if (command == "exit")
    {
        try
        {
            Environment.Exit(Convert.ToInt32(inputArr[1]));
        }
        catch (Exception){Console.WriteLine($"{inputArr[1]}: incorrect exit code");}

    }
    else
        {
            string log = Commands.RunCommand(command, inputArr);

            if (log.Length > 0)
                Console.WriteLine(log);
        }
    }