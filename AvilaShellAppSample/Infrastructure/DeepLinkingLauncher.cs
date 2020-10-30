using System;
using System.Threading.Tasks;
using AvilaShellAppSample.Monitoring;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AvilaShellAppSample.Infrastructure
{
    public class DeepLinkingLauncher : IDeepLinkingLauncher
    {
        private readonly IBrowser _browser;
        private readonly IEventTracker _eventTracker;

        public DeepLinkingLauncher()
        {
            _browser = new Browser();
            _eventTracker = new AppCenterEventTracker();
        }

        /* Facebook Page */
        // page => OK / Android 
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://page/115592608462989");
        // page => OK / iOS
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://profile/115592608462989");
        public async Task OpenFacebookPageAsync(string uri, string pageId)
        {
            try
            {
                var fbDlS = await IsFacebookDeepLinkingSupported();
                if (fbDlS)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Launcher.OpenAsync($"fb://profile/{pageId}");
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        await Launcher.OpenAsync($"fb://page/{pageId}");
                    }
                }
                else
                {
                    await _browser.OpenAsync(uri);
                }
            }
            catch (Exception ex)
            {
                // An error has occurred
                _eventTracker.Error(ex);

                await _browser.OpenAsync(uri);
            }
        }

        /* Facebook Post/News */
        // post => KO / Android
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://115592608462989_3623831304305751");
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://page/115592608462989/posts/3623831304305751");
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://page/115592608462989/posts?id=3623831304305751");
        //await Xamarin.Essentials.Launcher.OpenAsync("intent://www.facebook.com/115592608462989/posts/3623831304305751#Intent;package=com.facebook.katana;scheme=https;end");
        // post => OK but / Android (popin "Web / Other")
        //var debugPostUrl = "https://www.facebook.com/115592608462989/posts/3623831304305751/?extid=HepgIsLt5x4Rhgim&d=n";
        //await Xamarin.Essentials.Browser.OpenAsync(debugPostUrl);
        // post => OK / Android (alternative way)
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://facewebmodal/f?href=" + selectedNews.Url);
        // post => OK / iOS
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://profile/115592608462989/posts?id=3623831304305751");
        public async Task OpenFacebookPostAsync(string uri, string pageId, string postId)
        {
            try
            {
                var fbDlS = await IsFacebookDeepLinkingSupported();
                if (fbDlS)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Launcher.OpenAsync($"fb://profile/{pageId}/posts?id={postId}");
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        await Launcher.OpenAsync($"fb://facewebmodal/f?href={uri}");
                    }
                }
                else
                {
                    await _browser.OpenAsync(uri);
                }
            }
            catch (Exception ex)
            {
                // An error has occurred
                _eventTracker.Error(ex);

                await _browser.OpenAsync(uri);
            }
        }


        /* Facebook Event */
        // event => OK / Android
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://event/2720519278225723");
        // event => OK / iOS
        //await Xamarin.Essentials.Launcher.OpenAsync("fb://event?id=2720519278225723");
        public async Task OpenFacebookEventAsync(string uri, string eventId)
        {
            try
            {
                var fbDlS = await IsFacebookDeepLinkingSupported();
                if (fbDlS)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Launcher.OpenAsync($"fb://event?id={eventId}");
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        await Launcher.OpenAsync($"fb://event/{eventId}");
                    }
                }
                else
                {
                    await _browser.OpenAsync(uri);
                }
            }
            catch (Exception ex)
            {
                // An error has occurred
                _eventTracker.Error(ex);

                await _browser.OpenAsync(uri);
            }
        }

        private async Task<bool> IsFacebookDeepLinkingSupported()
        {
            return await Xamarin.Essentials.Launcher.CanOpenAsync("fb://");
        }
    }
}
