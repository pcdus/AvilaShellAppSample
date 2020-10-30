using System;
using System.Threading.Tasks;
using AvilaShellAppSample.Monitoring;
using Xamarin.Essentials;

namespace AvilaShellAppSample.Infrastructure
{
    public class Browser : IBrowser
    {
        private readonly IEventTracker _eventTracker;

        public Browser()
        {
            _eventTracker = new AppCenterEventTracker();
        }

        public Task OpenAsync(string uri)
        {
            try
            {
                return Xamarin.Essentials.Browser.OpenAsync(uri);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
                return Task.CompletedTask;
            }
        }

        public Task OpenAsync(string uri, BrowserLaunchMode launchMode)
        {
            try
            {
                return Xamarin.Essentials.Browser.OpenAsync(uri, launchMode);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
                return Task.CompletedTask;
            }
        }

        public Task OpenAsync(Uri uri)
        {
            try
            {
                return Xamarin.Essentials.Browser.OpenAsync(uri);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
                return Task.CompletedTask;
            }
        }

        public Task OpenAsync(Uri uri, BrowserLaunchMode launchMode)
        {
            try
            {
                return Xamarin.Essentials.Browser.OpenAsync(uri, launchMode);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
                return Task.CompletedTask;
            }
        }

    }
}
