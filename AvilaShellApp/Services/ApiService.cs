using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AvilaShellApp.Services.Abstractions;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace AvilaShellApp.Services
{
    public class ApiService : IApiService
    {
        readonly IClient _client;
        readonly INetworkService _networkService;

        public bool IsConnected { get; set; }

        public ApiService()
        {
            Debug.WriteLine("ApiService - Ctor()");

            _client = new Client();
            _networkService = new NetworkService();

            IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
            Debug.WriteLine($"ApiService - OnConnectivityChanged() - IsConnected = {IsConnected}");
        }

        public async Task<T> GetAsync<T>(Uri uri) where T : class
        {
            Debug.WriteLine("ApiService - GetAsync()");
            return await ProcessGetRequest<T>(uri);
        }

        #region With Retries

        public async Task<T> GetAndRetry<T>(Uri uri, int retryCount = 1, Func<Exception, int, Task> onRetry = null)
            where T : class
        {
            Debug.WriteLine("ApiService - GetAndRetry()");
            var func = new Func<Task<T>>(() => ProcessGetRequest<T>(uri));
            return await _networkService.Retry<T>(func, retryCount, onRetry);
        }

        public async Task<T> GetAndRetryWithTimeout<T>(Uri uri, int timeoutDelay = 30, int retryCount = 1, Func<Exception, int, Task> onRetry = null)
            where T : class
        {
            Debug.WriteLine("ApiService - GetAndRetryWithTimeout()");
            return await _networkService.RetryWithTimeout(IssueRequest<T>(uri), timeoutDelay, retryCount, onRetry);
        }

        public async Task<T> GetAndWaitAndRetry<T>(Uri uri, Func<int, TimeSpan> sleepDurationProvider, int retryCount, Func<Exception, TimeSpan, Task> onWaitAndRetry = null)
            where T : class
        {
            Debug.WriteLine("ApiService - GetAndWaitAndRetry()");
            var func = new Func<Task<T>>(() => ProcessGetRequest<T>(uri));
            return await _networkService.WaitAndRetry<T>(func, sleepDurationProvider, retryCount, onWaitAndRetry);
        }

        public async Task<T> GetAndWaitAndRetryWithTimeout<T>(Uri uri, Func<int, TimeSpan> sleepDurationProvider, int timeoutDelay = 30, int retryCount = 1, Func<Exception, TimeSpan, Task> onWaitAndRetry = null)
            where T : class
        {
            Debug.WriteLine("ApiService - GetAndWaitAndRetry()");
            return await _networkService.WaitAndRetryWithTimeout(IssueRequest<T>(uri), sleepDurationProvider, timeoutDelay, retryCount, onWaitAndRetry);
        }
        #endregion

        #region Inner Methods

        Func<CancellationToken, Task<T>> IssueRequest<T>(Uri uri) => ct => ProcessGetRequest<T>(ct, uri);

        async Task<T> ProcessGetRequest<T>(Uri uri)
        {
            Debug.WriteLine("ApiService - ProcessGetRequest(uri)");
            if (!IsConnected)
            {
                Debug.WriteLine("ApiService - ProcessGetRequest() - !IsConnected");
                throw new IOException("No internet access");
            }
            var response = await _client.Get(uri);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("ApiService - ProcessGetRequest() - !response.IsSuccessStatusCode");
                throw new WebException($"No success status code {response.StatusCode}");
            }
            var rawResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(rawResponse);
        }

        async Task<T> ProcessGetRequest<T>(CancellationToken ct, Uri uri)
        {
            Debug.WriteLine("ApiService - ProcessGetRequest(ct, uri)");
            if (!IsConnected)
            {
                Debug.WriteLine("ApiService - ProcessGetRequest() - !IsConnected");
                throw new IOException("No internet access");
            }

            var response = await _client.Get(uri, ct);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("ApiService - ProcessGetRequest() - !response.IsSuccessStatusCode");
                throw new WebException($"No success status code {response.StatusCode}");
            }
            var rawResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(rawResponse);
        }

        #endregion
    }
}
