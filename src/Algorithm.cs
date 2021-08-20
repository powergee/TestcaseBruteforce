using System;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace TestcaseBruteforce {
    enum ExitKind {
        ExitedNormally, ExceptionOccured, RuntimeErrorOccured, TimeLimitExceeded, MemoryLimitExceeded
    }

    record AlgorithmResult(
        ExitKind Kind,
        string Output,
        int? TotalTime,
        long? TotalMemoryInBytes,
        Exception Exception
    ) {
        public string GetMarkupString(string title, bool includeTimeAndMemory) {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(title)) {
                builder.Append($"[grey]{title} -[/] ");
            }
            builder.Append(Kind == ExitKind.ExitedNormally ? "[green]Passed[/]" : "[red]Failed[/]");
            if (includeTimeAndMemory) {
                builder.Append($" [grey]({TimeToMarkupString()}, {MemoryToMarkupString()})[/]");
            }
            return builder.ToString();
        }

        private string TimeToMarkupString() {
            return TotalTime == null ? "[underline]-ms[/]" : $"[underline]{TotalTime.Value}ms[/]";
        }

        private string MemoryToMarkupString() {
            if (TotalMemoryInBytes == null) {
                return "[underline]-B[/]";
            }

            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int unitIndex = 0;
            double bytes = (double)TotalMemoryInBytes.Value;
            while ((long)(bytes/1024) > 0) {
                bytes /= 1024;
                ++unitIndex;
            }
            return $"[underline]{bytes.ToString("0.##")}{units[unitIndex]}[/]";
        }
    }

    class Algorithm {
        public string Command { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public string Input { get; set; }

        public AlgorithmResult Execute() {
            string fileName, arguments;
            if (Command.Contains(' ')) {
                fileName = Command.Substring(0, Command.IndexOf(' '));
                arguments = Command.Substring(Command.IndexOf(' ')+1);
            } else {
                fileName = Command;
                arguments = "";
            }

            ProcessStartInfo startInfo = new ProcessStartInfo{
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = fileName,
                Arguments = arguments
            };

            Process process = new Process();
            ExitKind kind = ExitKind.ExitedNormally;
            string output = null;
            int? totalTime = null;
            long? totalMemoryInBytes = null;
            Exception exception = null;

            try {
                process.StartInfo = startInfo;
                process.Start();

                if (!string.IsNullOrWhiteSpace(Input)) {
                    process.StandardInput.Write(Input);
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                }

                Stopwatch timer = new Stopwatch();
                timer.Start();

                bool tle = false, mle = false;

                try {
                    while (!tle && !mle && !process.HasExited) {
                        process.Refresh();
                        totalMemoryInBytes = process.PeakVirtualMemorySize64;

                        if (TimeLimit < timer.ElapsedMilliseconds) {
                            tle = true;
                        } else if (MemoryLimit*1024L*1024L < totalMemoryInBytes) {
                            mle = true;
                        }  else {
                            Thread.Sleep(10);
                        }
                    }
                } catch (InvalidOperationException) {
                    // process.Refresh() can throw InvalidOperationException
                    // if the process has exited. It needs to be ignored.
                }

                if (!process.HasExited) {
                    process.Kill();
                }

                totalTime = (int)timer.ElapsedMilliseconds;
                timer.Stop();

                if (tle) {
                    kind = ExitKind.TimeLimitExceeded;
                } else if (mle) {
                    kind = ExitKind.MemoryLimitExceeded;
                } else if (process.ExitCode != 0) {
                    kind = ExitKind.RuntimeErrorOccured;
                } else {
                    output = process.StandardOutput.ReadToEnd();
                }
            } catch (Exception e) {
                kind = ExitKind.ExceptionOccured;
                exception = e;
            } finally {
                process.Dispose();
            }

            return new AlgorithmResult(kind, output, totalTime, totalMemoryInBytes, exception);
        }
    }
}