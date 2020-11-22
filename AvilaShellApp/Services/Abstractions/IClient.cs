using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AvilaShellApp.Services.Abstractions
{
    public interface IClient
    {
        Task<HttpResponseMessage> Get(Uri uri);
        Task<HttpResponseMessage> Get(Uri uri, CancellationToken ct);
        Task<HttpResponseMessage> Post(Uri uri, HttpContent content);
        Task<HttpResponseMessage> Post(Uri uri, HttpContent content, CancellationToken ct);
    }
}
