using System;
using System.IO;

namespace Assignment_1_Shell_Interface
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Demo";

            EnterCommandLoop();


        }


        static void EnterCommandLoop()
        {
            Console.Write($"{Environment.CurrentDirectory}> ");
            // Read command
            var CommandLine = Console.ReadLine().Split(' ');
            var Command = CommandLine[0].Trim();
            var Paramaters = CommandLine.Length >= 2 ? CommandLine[1].Trim(): null;

            switch (Command)
            { 
                case "pwd":
                    Pwd();
                    break;
                case "exit":
                    Exit();
                    break;

                case "ls":
                    ListDirectory();
                    break;

                case "cat":
                    ReadFileContents(Paramaters);
                    break;
            }

            // Perform Action

            // Enter Loop
            EnterCommandLoop();
        }

        private static void Pwd()
        {
            // Write current working directory to Console
            Console.WriteLine(Environment.CurrentDirectory);
        }

        private static void Exit()
        {
            // Nice Exit code
            Environment.Exit(0);
        }

        private static void ListDirectory()
        {
            // List directories and files in the current dir
            var curDir = new DirectoryInfo(Environment.CurrentDirectory);

            foreach (var dirs in curDir.GetDirectories())
            {
                Console.WriteLine(dirs.Name);
            }

            foreach (var file in curDir.GetFiles())
            {
                Console.WriteLine(file.Name);
            }
        }

        private static void ReadFileContents(params String[] Parms)
        {
            // Read file contents from parms variable 
            // Only reading 1 file
            var fileName = Parms[0];

            

            Console.WriteLine($"File Name: {Parms[0]}");
        }
    }
}
