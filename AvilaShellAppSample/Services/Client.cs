using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AvilaShellAppSample.Services.Abstractions;

namespace AvilaShellAppSample.Services
{
    public class Client : IClient
    {
            readonly HttpClient _client;

            public Client()
            {
                _client = new HttpClient
                {
                    BaseAddress = new Uri(ApiConfig.FbApiBaseUrl)
                };
            }

            public async Task<HttpResponseMessage> Get(Uri uri) => await _client.GetAsync(uri);
            public async Task<HttpResponseMessage> Get(Uri uri, CancellationToken ct) => await _client.GetAsync(uri, ct);
            public async Task<HttpResponseMessage> Post(Uri uri, HttpContent content) => await _client.PostAsync(uri, content);
            public async Task<HttpResponseMessage> Post(Uri uri, HttpContent content, CancellationToken ct) => await _client.PostAsync(uri, content, ct);
    }
}
