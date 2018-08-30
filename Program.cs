using System;
using System.IO;
using System.Diagnostics;

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
            var Paramaters = CommandLine.Length >= 2 ? CommandLine[1].Trim(): "";

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
            }

            // Perform Action

            // Enter Loop
            EnterCommandLoop();
        }

        private static void Exit()
        {
            // Nice Exit code
            Environment.Exit(0);
        }

        private static void SpawnShellProcess(string command, string parameters = null)
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
