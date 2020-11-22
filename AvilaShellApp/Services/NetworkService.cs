using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AvilaShellApp.Services.Abstractions;
using Polly;

namespace AvilaShellApp.Services
{
    public class NetworkService : INetworkService
    {
        #region Retry

        public async Task<T> Retry<T>(Func<Task<T>> func)
        {
            return await RetryInner(func);
        }

        public async Task<T> Retry<T>(Func<Task<T>> func, int retryCount)
        {
            return await RetryInner(func, retryCount);
        }

        public async Task<T> Retry<T>(Func<Task<T>> func, int retryCount, Func<Exception, int, Task> onRetry)
        {
            return await RetryInner(func, retryCount, onRetry);
        }

        public async Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func)
        {
            return await RetryInnerWithTimeout(func);
        }

        public async Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeoutDelay)
        {
            return await RetryInnerWithTimeout(func, timeoutDelay);
        }

        public async Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeoutDelay, int retryCount)
        {
            return await RetryInnerWithTimeout(func, timeoutDelay, retryCount);
        }

        public async Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeoutDelay, int retryCount, Func<Exception, int, Task> onRetry)
        {
            return await RetryInnerWithTimeout(func, timeoutDelay, retryCount, onRetry);
        }

        #endregion

        #region WaitAndRetry 

        public async Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider);
        }

        public async Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider, retryCount);
        }

        public async Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount, Func<Exception, TimeSpan, Task> onRetryAsync)
        {
            return await WaitAndRetryInner<T>(func, sleepDurationProvider, retryCount, onRetryAsync);
        }

        public async Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider)
        {
            return await WaitAndRetryInnerWithTimeout<T>(func, sleepDurationProvider);
        }

        public async Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay)
        {
            return await WaitAndRetryInnerWithTimeout<T>(func, sleepDurationProvider, timeoutDelay);
        }

        public async Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay, int retryCount)
        {
            return await WaitAndRetryInnerWithTimeout<T>(func, sleepDurationProvider, timeoutDelay, retryCount);
        }

        public async Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay, int retryCount, Func<Exception, TimeSpan, Task> onRetryAsync)
        {
            return await WaitAndRetryInnerWithTimeout<T>(func, sleepDurationProvider, timeoutDelay, retryCount, onRetryAsync);
        }

        #endregion

        #region Inner Methods

        internal async Task<T> RetryInner<T>(Func<Task<T>> func, int retryCount = 1, Func<Exception, int, Task> onRetry = null)
        {
#if DEBUG
            Debug.WriteLine("NetworkService - RetryInner()");
#endif
            var onRetryInner = new Func<Exception, int, Task>((e, i) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    Debug.WriteLine("NetworkService - RetryInner() - onRetryInner");
                    Debug.WriteLine($"Retry #{i} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            return await Policy
                .Handle<Exception>()
                .RetryAsync(retryCount, onRetry ?? onRetryInner)
                .ExecuteAsync<T>(func);
        }

        internal async Task<T> RetryInnerWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeoutDelay = 30, int retryCount = 1, Func<Exception, int, Task> onRetry = null)
        {
#if DEBUG
            Debug.WriteLine("NetworkService - RetryInnerWithTimeout()");
#endif
            var onRetryInner = new Func<Exception, int, Task>((e, i) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    Debug.WriteLine("NetworkService - RetryInnerWithTimeout() - onRetryInner");
                    Debug.WriteLine($"Retry #{i} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            var retryPolicy = Policy
                .Handle<Exception>()
                .RetryAsync(retryCount, onRetry ?? onRetryInner);

            var timeoutPolicy = Policy
                .TimeoutAsync(TimeSpan.FromSeconds(timeoutDelay));

            var policyWrap = timeoutPolicy.WrapAsync(retryPolicy);

            return await policyWrap.ExecuteAsync(
                            async ct => await func(ct),
                            CancellationToken.None);
        }

        internal async Task<T> WaitAndRetryInner<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount = 1, Func<Exception, TimeSpan, Task> onRetryAsync = null)
        {
#if DEBUG
            Debug.WriteLine("NetworkService - WaitAndRetryInner()");
#endif
            var onRetryInner = new Func<Exception, TimeSpan, Task>((e, t) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    Debug.WriteLine("NetworkService - WaitAndRetryInner() - onRetryInner");
                    Debug.WriteLine($"Retrying in {t.ToString("g")} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            return await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(retryCount, sleepDurationProvider, onRetryAsync ?? onRetryInner)
                .ExecuteAsync<T>(func);

        }

        internal async Task<T> WaitAndRetryInnerWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay = 30, int retryCount = 1, Func<Exception, TimeSpan, Task> onRetryAsync = null)
        {
#if DEBUG
            Debug.WriteLine("NetworkService - WaitAndRetryInnerWithTimeout()");
#endif
            var onRetryInner = new Func<Exception, TimeSpan, Task>((e, t) =>
            {
                return Task.Factory.StartNew(() => {
#if DEBUG
                    Debug.WriteLine("NetworkService - WaitAndRetryInnerWithTimeout() - onRetryInner");
                    Debug.WriteLine($"Retrying in {t.ToString("g")} due to exception '{(e.InnerException ?? e).Message}'");
#endif
                });
            });

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(retryCount, sleepDurationProvider, onRetryAsync ?? onRetryInner);

            var timeoutPolicy = Policy
                .TimeoutAsync(TimeSpan.FromSeconds(timeoutDelay));

            var policyWrap = timeoutPolicy.WrapAsync(retryPolicy);

            return await policyWrap.ExecuteAsync(
                            async ct => await func(ct),
                            CancellationToken.None);

        }

        #endregion
    }
}
