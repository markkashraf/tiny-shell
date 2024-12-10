using System.Collections;

namespace sys
{
    public static class Sys
    {

        private static IDictionary env;
        private static string cwd;
        public static string Cwd
        {
            get => cwd;
            set
            {
                if (Directory.Exists(value))
                    cwd = value;
            }
        }


        static Sys()
        {
            try
            {
                env = Environment.GetEnvironmentVariables();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: couldn't read environment variables \"{e}\".");
                Environment.Exit(1);
            }


            cwd = System.IO.Directory.GetCurrentDirectory();
        }

        /* get the directories in the PATH environment variable */

        public static string[]? GetPathArr() => env["PATH"]!.ToString()!.Split(':');
        // get the directories in the HOME environment variable
        public static string[]? GetHomeArr() => env["HOME"]!.ToString()!.Split('/');



    }
}

