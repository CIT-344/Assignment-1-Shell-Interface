using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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
            var Paramaters = CommandLine.Length >= 2 ? BuildParams(CommandLine.Skip(1)): "";

            switch (Command)
            { 
                case "pwd":
                    SpawnShellProcess("pwd");
                    break;
                case "exit":
                    Exit();
                    break;

                case "ls":
                    SpawnShellProcess("ls");
                    break;

                case "cat":
                    SpawnShellProcess(String.Concat(Command, " ", Paramaters));
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "cls":
                    Console.Clear();
                    break;

                case "date":
                    SpawnShellProcess("date");
                    break;

                case "cd":
                    ChangeWorkingDirectory(Paramaters);
                    break;

                case "mkdir":
                    SpawnShellProcess(String.Concat(Command, " ", Paramaters));
                    break;

                case "rm":
                    SpawnShellProcess(String.Concat(Command, " ", Paramaters));
                    break;

                default:
                    Console.WriteLine("Unsupported command");
                    break;
            }

            // Perform Action

            // Enter Loop
            EnterCommandLoop();
        }

        private static string BuildParams(IEnumerable<string> enumerable)
        {
            var builder = new StringBuilder();

            foreach (var spacer in enumerable)
            {
                builder.Append(spacer + " ");
            }

            return builder.ToString().Trim(); 
        }

        private static void ChangeWorkingDirectory(string paramaters)
        {
            // Change the Environment.CurrentDirectory value
            var paths = paramaters.Split('/');
            var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
            var tempDirInfo = dirInfo;

            foreach (var pathItem in paths)
            {
                if (pathItem == "..")
                {
                    // Go up to the current parents dir
                    tempDirInfo = tempDirInfo.Parent;
                }
                else
                {
                    // Some other named dir
                    // First check if that dir exists in the current dir structure
                    if (tempDirInfo.GetDirectories().Where(x => x.Name == pathItem).Count() != 0)
                    {
                        // There is a dir with a matching name
                        tempDirInfo = tempDirInfo.GetDirectories().Where(x => x.Name == pathItem).FirstOrDefault();
                    }
                    else
                    {
                        // Throw some kind of error about pathing
                        
                    }
                }
            }


            Environment.CurrentDirectory = tempDirInfo != null ? tempDirInfo.FullName : dirInfo.FullName;

        }

        private static void Exit()
        {
            // Nice Exit code
            Environment.Exit(0);
        }

        private static void SpawnShellProcess(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo("powershell");
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.Arguments = command;
            
            psi.WorkingDirectory = Environment.CurrentDirectory;

            Process p = new Process();
            p.StartInfo = psi;

            p.Start();

            var output = p.StandardOutput.ReadToEnd().Trim();

            Console.WriteLine(output + "\n");
        }
    }
}
