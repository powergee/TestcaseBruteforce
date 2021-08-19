using System;
using Spectre.Console;

namespace TestcaseBruteforce {
    class StatusTable : IDisposable {
        private int rowCount = 0;
        private int algCount;
        private bool disposedValue;

        public AlgorithmResult GeneratorResult {
            set {
                Console.Write($"Generator: {value.Kind.ToString()}({value.TotalTime} ms, {value.TotalMemoryInBytes} Bytes), ");
            }
        }

        public AlgorithmResult this[int index] {
            set {
                Console.Write($"Algorithm {index+1}: {value.Kind.ToString()}({value.TotalTime} ms, {value.TotalMemoryInBytes} Bytes), ");
            }
        }

        public StatusTable(int algCount) {
            this.algCount = algCount;
        }

        public void AppendRow() {
            Console.Write($"\n[{++rowCount}] ");
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~StatusTable()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}