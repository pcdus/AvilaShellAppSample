using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvilaShellAppSample.Services.Abstractions
{
    public interface INetworkService
    {
        Task<T> Retry<T>(Func<Task<T>> func);
        Task<T> Retry<T>(Func<Task<T>> func, int retryCount);
        Task<T> Retry<T>(Func<Task<T>> func, int retryCount, Func<Exception, int, Task> onRetry);
        Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func);
        Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeoutDelay);
        Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeoutDelay, int retryCount);
        Task<T> RetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeoutDelay, int retryCount, Func<Exception, int, Task> onRetry);
        Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider);
        Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount);
        Task<T> WaitAndRetry<T>(Func<Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int retryCount, Func<Exception, TimeSpan, Task> onRetryAsync);
        Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider);
        Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay);
        Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay, int retryCount);
        Task<T> WaitAndRetryWithTimeout<T>(Func<CancellationToken, Task<T>> func, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay, int retryCount, Func<Exception, TimeSpan, Task> onRetryAsync);
    }
}
