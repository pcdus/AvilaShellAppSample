using System;
using System.Threading.Tasks;

namespace AvilaShellAppSample.Services.Abstractions
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(Uri uri)
        where T : class;

        #region With Retries

        Task<T> GetAndRetry<T>(Uri uri, int retryCount = 1, Func<Exception, int, Task> onRetry = null)
            where T : class;
        Task<T> GetAndRetryWithTimeout<T>(Uri uri, int timeoutDelay = 30, int retryCount = 1, Func<Exception, int, Task> onRetry = null)
            where T : class;
        Task<T> GetAndWaitAndRetry<T>(Uri uri, Func<int, TimeSpan> sleepDurationProvider, int retryCount = 1, Func<Exception, TimeSpan, Task> onWaitAndRetry = null)
            where T : class;
        Task<T> GetAndWaitAndRetryWithTimeout<T>(Uri uri, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay = 30, int retryCount = 1, Func<Exception, TimeSpan, Task> onWaitAndRetry = null)
            where T : class;

        #endregion
    }
}
