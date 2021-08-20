using System.Linq;
using Spectre.Console;

namespace TestcaseBruteforce {
    enum Verdict {
        Undefined, Accepted, WrongAnswer, TimeLimitExceeded, MemoryLimitExceeded, RuntimeError
    }

    class TestLog {
        public AlgorithmResult GeneratorResult { get; set; }
        public AlgorithmResult[] TestResults { get; set; }
        public bool? IsAccepted { get; set; }
        public Verdict Verdict { get; set; }
        public string[] TestOutputs => TestResults.Select(ar => ar.Output).ToArray();

        public TestLog(int algCount) {
            TestResults = new AlgorithmResult[algCount];
            Verdict = Verdict.Undefined;
        }

        public string GetMarkupString(bool includeTimeAndMemory) {
            string[] markups = new string[TestResults.Length+2];
            markups[0] = GeneratorResult.GetMarkupString("Generator", includeTimeAndMemory);
            for (int i = 0; i < TestResults.Length; ++i) {
                markups[i+1] = TestResults[i].GetMarkupString($"Algorithm {i+1}", includeTimeAndMemory);
            }

            string vdMarkup = "";
            switch (Verdict) {
                case Verdict.Accepted:
                    vdMarkup = "[underline green]Accepted[/]";
                    break;
                case Verdict.WrongAnswer:
                    vdMarkup = "[underline red]Wrong Answer[/]";
                    break;
                case Verdict.TimeLimitExceeded:
                    vdMarkup = "[underline yellow]Time Limit Exceeded[/]";
                    break;
                case Verdict.MemoryLimitExceeded:
                    vdMarkup = "[underline yellow]Memory Limit Exceeded[/]";
                    break;
                case Verdict.RuntimeError:
                    vdMarkup = "[underline yellow]Runtime Error[/]";
                    break;
                case Verdict.Undefined:
                    vdMarkup = "[grey]-[/]";
                    break;
            }
            markups[TestResults.Length+1] = "Verdict - " + vdMarkup;

            return string.Join(" | ", markups);
        }
    }
}