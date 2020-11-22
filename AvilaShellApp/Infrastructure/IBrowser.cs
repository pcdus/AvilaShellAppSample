using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AvilaShellApp.Infrastructure
{
    public interface IBrowser
    {
        Task OpenAsync(string uri);
        Task OpenAsync(string uri, BrowserLaunchMode launchMode);
        Task OpenAsync(Uri uri);
        Task OpenAsync(Uri uri, BrowserLaunchMode launchMode);
    }
}
