using System;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Spectre.Console;

namespace TestcaseBruteforce {
    class TestLog {
        public AlgorithmResult GeneratorResult { get; set; }
        public AlgorithmResult[] TestResults { get; set; }
        public bool? IsAccepted { get; set; }
        public string[] TestOutputs => TestResults.Select(ar => ar.Output).ToArray();

        public TestLog(int algCount) {
            TestResults = new AlgorithmResult[algCount];
        }

        public string GetMarkupString(bool includeTimeAndMemory) {
            string[] markups = new string[TestResults.Length+2];
            markups[0] = GeneratorResult.GetMarkupString("Generator", includeTimeAndMemory);
            for (int i = 0; i < TestResults.Length; ++i) {
                markups[i+1] = TestResults[i].GetMarkupString($"Algorithm {i+1}", includeTimeAndMemory);
            }

            string vdMarkup;
            if (IsAccepted == true) {
                vdMarkup = "[underline green]Accepted[/]";
            } else if (IsAccepted == false) {
                vdMarkup = "[underline red]Wrong Answer[/]";
            } else {
                vdMarkup = "[grey]-[/]";
            }
            markups[TestResults.Length+1] = "Verdict - " + vdMarkup;

            return string.Join(" | ", markups);
        }
    }
}