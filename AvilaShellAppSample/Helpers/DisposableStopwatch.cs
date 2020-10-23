using System;
using System.Diagnostics;

namespace AvilaShellAppSample.Helpers
{
    public class DisposableStopwatch : IDisposable
    {
        private readonly Stopwatch _sw;
        private readonly Action<TimeSpan> _action;

        public DisposableStopwatch(Action<TimeSpan> action)
        {
            _action = action;
            _sw = Stopwatch.StartNew();
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _sw.Stop();
                _action?.Invoke(_sw.Elapsed);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
