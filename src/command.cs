using sys;
namespace commands
{
    delegate string Command(string[] s);
    public static class Commands
    {
        private static string? cwd;
        private static Dictionary<string, Command> commandDict;

        private static string[]? paths = null;
        private static string[]? home = null;
        


        static Commands()
        {
            commandDict = new Dictionary<string, Command>();
            cwd = Sys.Cwd;
            paths = Sys.GetPathArr();
            home = Sys.GetHomeArr();

            commandDict["echo"] = echo;
            commandDict["type"] = type;
            commandDict["exit"] = exit;
            commandDict["pwd"] = pwd;
            commandDict["cd"] = cd;
            commandDict["cat"] = cat;
        }

        public static bool ContainsCommand(string command) => commandDict.ContainsKey(command);

        public static string RunCommand(string s, string[] paramList)
        {
            if (!Commands.ContainsCommand(s))
                return $"{s}: not found";
            else
                return commandDict[s].Invoke(paramList);
        }



        private static string cat(string[] s)
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

        private static string pwd(string[] s)
        {
            return cwd!;
        }

        private static string cd(string[] s)
        {
            string[] dirArr = s[1].Split('/');
            string[] cwdArr = cwd!.Split('/');
            LinkedList<string> dirDq = new LinkedList<string>();


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
                    foreach (string x in home!)
                        if (x != "")
                            dirDq.AddLast(x);
                }
                else if (p != "")
                    dirDq.AddLast(p);
            }

            string fixedDir = "";

            while (dirDq.Count > 0)
            {
                fixedDir += "/" + dirDq.First!.Value;
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

        private static string exit(string[] s)
        {
            return "";
        }

        private static string type(string[] s)
        {
            string result;
            string command = s[1];
            if (command == "cat")
            {
                foreach (string p in paths!)
                {
                    string[] cwdFiles;
                    try { cwdFiles = Directory.GetFiles(p); }
                    catch (Exception) { continue; }

                    foreach (string f in cwdFiles)
                    {
                        if (f.EndsWith("/" + command))
                        {
                            return $"{command} is {f}"; ;
                        }
                    }
                }

            }

            if (commandDict.ContainsKey(command)) return $"{command} is a shell builtin";



            foreach (string p in paths!)
            {
                string[] cwdFiles;

                try
                { cwdFiles = Directory.GetFiles(p); }
                catch (Exception)
                { continue; }

                foreach (string f in cwdFiles)
                {
                    if (f.EndsWith('/' + command)) return $"{command} is {f}";

                }
            }


            result = $"{s[1]}: not found";

            return result;
        }

        private static string echo(string[] s)
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
    }
}