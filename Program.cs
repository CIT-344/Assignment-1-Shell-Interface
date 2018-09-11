using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
            string pattern = @"^[A-Z][:]"; // Catch things like C: D: Z: not AB: etc

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
                else if (Regex.IsMatch(pathItem, pattern))
                {
                    // Drive change command
                    tempDirInfo = new DirectoryInfo(String.Concat(pathItem, "/"));
                    break; // Jump out of this loop because our drive has completely changed this is what the cmd does
                } else if (paramaters.Trim().Length == 0) // Nothing was sent in
                {
                    tempDirInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                    break;
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
            
            Environment.CurrentDirectory = tempDirInfo != null && tempDirInfo.Exists ? tempDirInfo.FullName : dirInfo.FullName;

        }

        private static void Exit()
        {
            // Nice Exit code
            Environment.Exit(0);
        }

        private static void SpawnShellProcess(string command)
        {
            var oldTitle = Console.Title;
            Console.Title = $"Running '{command}'";
            
            ProcessStartInfo psi = new ProcessStartInfo("powershell")
            {
                CreateNoWindow = true, // Don't launch a blue powershell window
                RedirectStandardOutput = true, // Allow catching the ouput from process
                Arguments = command, // The literal powershell command with any params attached to the end

                WorkingDirectory = Environment.CurrentDirectory // Set the process directory to the program dir, I maintain curDirectory outside of the process
            };

            Process p = new Process
            {
                StartInfo = psi
            };

            p.Start(); // Launch the process in the background

            var output = p.StandardOutput.ReadToEnd().Trim(); // Read all it's output 

            Console.WriteLine(output + "\n"); // Write output to console window 

            Console.Title = oldTitle;
        }
    }
}
