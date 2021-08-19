﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using Spectre.Console;

namespace TestcaseBruteforce {
    class Program {
        static async Task<int> Main(string[] args) {
            AnsiConsole.Render(new FigletText("TCBRUTE").Color(Color.SkyBlue1));

            RootCommand rootCommand = new RootCommand();

            Option genOption = new Option<string>(
                new string[] { "-g", "--generator" }, 
                description: "The testcase generator file path"
            );
            genOption.IsRequired = true;
            rootCommand.AddOption(genOption);

            Option algOption = new Option<string[]>(
                new string[] { "-a", "--algorithms" }, 
                description: "The executables to find testcases that make them fail"
            );
            algOption.IsRequired = true;
            rootCommand.AddOption(algOption);

            Option outOption = new Option<string>(
                new string[] { "-o", "--out-path" }, 
                description: "Path to save a testcase (if it is not specified, Program prints it on the terminal and exits.)"
            );
            rootCommand.AddOption(outOption);

            Option timeOption = new Option<int>(
                new string[] { "-t", "--time-limit" },
                description: "The time limit (ms)",
                getDefaultValue: () => 2000
            );
            rootCommand.AddOption(timeOption);

            Option memoryOption = new Option<int>(
                new string[] { "-m", "--memory-limit" },
                description: "The memory limit (MB)",
                getDefaultValue: () => 512
            );
            rootCommand.AddOption(memoryOption);

            rootCommand.Name = "tcbrute";
            rootCommand.Description = "Do bruteforce to find a test case that make my algorithm fail.";

            rootCommand.Handler = CommandHandler.Create<string, string[], string, int, int>(Bruteforces);
            return await rootCommand.InvokeAsync(args);
        }

        static void Bruteforces(string generator, string[] algorithms, string outPath, int timeLimit, int memoryLimit) {
            if (algorithms.Length <= 1) {
                AnsiConsole.Render(new Markup("[underline red]Specify 2 or more algorithms.\n[/]"));
                return;
            }

            try {
                Table settingTable = GetSettingTable(generator, algorithms, outPath, timeLimit, memoryLimit);
                AnsiConsole.Render(settingTable);

                Algorithm genAlg = new Algorithm() {
                    Command = generator,
                    TimeLimit = timeLimit,
                    MemoryLimit = memoryLimit
                };

                List<Algorithm> algos = new List<Algorithm>();
                foreach (string algo in algorithms) {
                    algos.Add(new Algorithm() {
                        Command = algo,
                        TimeLimit = timeLimit,
                        MemoryLimit = memoryLimit
                    });
                }

                StatusTable status = new StatusTable(algos.Count);
                string input = null;
                string[] outputs = new string[algos.Count];
                string found = null;
                Exception excepOccured = null;

                while (found == null) {
                    outputs = new string[algos.Count];
                    status.AppendRow();

                    AlgorithmResult genResult = genAlg.Execute();
                    status.GeneratorResult = genResult;
                    if (genResult.Kind != ExitKind.ExitedNormally) {
                        excepOccured = genResult.Exception;
                        break;
                    }
                    input = genResult.Output;

                    bool errorOccured = false;
                    foreach (var algoPair in algos.Select((Algo, Index) => (Algo, Index))) {
                        algoPair.Algo.Input = input;
                        AlgorithmResult algResult = algoPair.Algo.Execute();
                        status[algoPair.Index] = algResult;
                        outputs[algoPair.Index] = algResult.Output;

                        if (algResult.Kind != ExitKind.ExitedNormally) {
                            errorOccured = true;
                            excepOccured = algResult.Exception;
                        }
                    }

                    if (errorOccured || !ValidateOutputs(outputs)) {
                        found = genResult.Output;
                    }
                }
                status.Dispose();
                AnsiConsole.WriteLine();

                if (found != null) {
                    AnsiConsole.Render(GetResultTable(input, outputs));
                }
                if (excepOccured != null) {
                    ExceptionDispatchInfo.Capture(excepOccured).Throw();
                }
            } catch (Exception e) {
                AnsiConsole.Render(new Markup("[underline bold red]An exception has occured while judging.\n[/]"));
                AnsiConsole.WriteException(e);
            }
        }

        static Table GetSettingTable(string generator, string[] algorithms, string outPath, int timeLimit, int memoryLimit) {
            Table settingTable = new Table();
            settingTable.Title = new TableTitle("[underline bold]Setting[/]");

            settingTable.AddColumn(new TableColumn(new Markup("[bold]Generator[/]")).Centered());
            for (int i = 1; i <= algorithms.Length; ++i) {
                settingTable.AddColumn(new TableColumn(new Markup($"[bold]Algorithm {i}[/]")).Centered());
            }
            settingTable.AddColumn(new TableColumn(new Markup("[bold]Out[/]")).Centered());
            settingTable.AddColumn(new TableColumn(new Markup("[bold]Time Limit (ms)[/]")).Centered());
            settingTable.AddColumn(new TableColumn(new Markup("[bold]Memory Limit (MB)[/]")).Centered());

            List<Markup> rowCells = new List<Markup>();
            rowCells.Add(new Markup($"[green]{generator}[/]"));
            foreach (string alg in algorithms) {
                rowCells.Add(new Markup($"[blue]{alg}[/]"));
            }
            rowCells.Add(new Markup($"[red]{(string.IsNullOrEmpty(outPath) ? "(Terminal)" : outPath)}[/]"));
            rowCells.Add(new Markup($"{timeLimit.ToString()}"));
            rowCells.Add(new Markup($"{memoryLimit.ToString()}"));
            settingTable.AddRow(rowCells);

            return settingTable;
        }

        static Table GetResultTable(string input, string[] outputs) {
            Table resultTable = new Table();
            resultTable.Title = new TableTitle("[underline bold]A Testcase Has Found.[/]");

            resultTable.AddColumn(new TableColumn(new Markup("[bold]Kind[/]")).RightAligned());
            resultTable.AddColumn(new TableColumn(new Markup("[bold]Text[/]")).LeftAligned());

            resultTable.AddRow(
                new Markup("[bold]Input[/]"),
                new Markup($"[green]{input}[/]")
            );

            for (int i = 0; i < outputs.Length; ++i) {
                resultTable.AddRow(
                    new Markup($"[bold]Algorithm {i+1}[/]"),
                    new Markup($"[blue]{outputs[i]}[/]")
                );
            }

            return resultTable;
        }

        static string[] Tokenize(string plain) {
            char[] delimiters = { ' ', '\n', '\r' };
            return plain.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        static bool ValidateOutputs(string[] outputs) {
            string[][] tokens = outputs.Select(Tokenize).ToArray();

            for (int i = 1; i < outputs.Length; ++i) {
                if (!ValidateTokens(tokens[0], tokens[i])) {
                    return false;
                }
            }
            return true;
        }

        static bool ValidateTokens(string[] tokens1, string[] tokens2) {
            if (tokens1.Length != tokens2.Length) {
                return false;
            }
            for (int i = 0; i < tokens1.Length; ++i) {
                if (!String.Equals(tokens1[i], tokens2[i])) {
                    return false;
                }
            }
            return true;
        }
    }
}