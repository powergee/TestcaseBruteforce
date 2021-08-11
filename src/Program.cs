using System;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Spectre.Console;

namespace TestcaseBruteforce {
    class Program {
        static async Task<int> Main(string[] args) {
            AnsiConsole.Render(new FigletText("TCBRUTE").Color(Color.SkyBlue1));

            RootCommand rootCommand = new RootCommand();

            Option genOption = new Option<FileInfo>(
                new string[] { "-g", "--generator" }, 
                description: "The testcase generator file path"
            );
            genOption.IsRequired = true;
            rootCommand.AddOption(genOption);

            Option algOption = new Option<FileInfo[]>(
                new string[] { "-a", "--algorithms" }, 
                description: "The executables to find testcases that make them fail"
            );
            algOption.IsRequired = true;
            rootCommand.AddOption(algOption);

            Option timeOption = new Option<int>(
                new string[] { "-t", "--time-limit" },
                description: "The time limit (ms)",
                getDefaultValue: () => 1000
            );
            rootCommand.AddOption(timeOption);

            Option memoryOption = new Option<int>(
                new string[] { "-m", "--memory-limit" },
                description: "The memory limit (MB)",
                getDefaultValue: () => 1024
            );
            rootCommand.AddOption(memoryOption);

            rootCommand.Name = "tcbrute";
            rootCommand.Description = "Do bruteforce to find a test case that make my algorithm fail.";

            rootCommand.Handler = CommandHandler.Create<FileInfo, FileInfo[], int, int>(Bruteforces);
            return await rootCommand.InvokeAsync(args);
        }

        static void Bruteforces(FileInfo generator, FileInfo[] algorithms, int timeLimit, int memoryLimit) {
            Console.WriteLine(generator.ToString());
            foreach (FileInfo info in algorithms) {
                Console.WriteLine(info.ToString());
            }
            Console.WriteLine(timeLimit);
            Console.WriteLine(memoryLimit);
        }
    }
}
