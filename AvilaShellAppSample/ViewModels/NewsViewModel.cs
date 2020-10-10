using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AvilaShellAppSample.Services;
using AvilaShellAppSample.Models;
using System.Diagnostics;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace AvilaShellAppSample.ViewModels
{
    public class NewsViewModel : AvilaViewModelBase
    {
        private readonly IDataService _dataService;

        private static readonly string _avilaFacebookPageId = "115592608462989";

        ObservableCollection<News> news = null;
        public ObservableCollection<News> News
        {
            get { return news; }
            set { SetProperty(ref news, value); }
        }

        ObservableCollection<Event> events = null;
        public ObservableCollection<Event> Events
        {
            get { return events; }
            set { SetProperty(ref events, value); }
        }

        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        bool hasEmptyData = true;
        public bool HasEmptyData
        {
            get { return hasEmptyData; }
            set { SetProperty(ref hasEmptyData, value); }
        }

        public AsyncCommand RefreshCommand => new AsyncCommand(this.RefreshAsync);
        public AsyncCommand<News> OpenNewsCommand => new AsyncCommand<News>(this.OpenNewsAsync);
        public AsyncCommand<Event> OpenEventCommand => new AsyncCommand<Event>(this.OpenEventAsync);

        public NewsViewModel()
        {
            Title = "News";
            News = new ObservableCollection<News>();

            _dataService = new DataService();

            Task.Run(async () => await GetNewsAsync());
        }

        private async Task GetNewsAsync()
        {
            IsBusy = true;
            await Task.Delay(750);
            //await Task.Delay(2500);

            var _events = await _dataService.GetEvents();
            Events = new ObservableCollection<Event>(_events);
            var _news = await _dataService.GetNews();
            News = new ObservableCollection<News>(_news);


            if (News.Count > 0 || Events.Count > 0)
                HasEmptyData = false;

            IsBusy = false;
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await GetNewsAsync();
            IsRefreshing = false;
        }

        private async Task OpenNewsAsync(News selectedNews)
        {
            // page => OK / Android 
            //await Xamarin.Essentials.Launcher.OpenAsync("fb://page/115592608462989");
            // page => OK / iOS
            //await Xamarin.Essentials.Launcher.OpenAsync("fb://profile/115592608462989");

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

            if (selectedNews == null)
                return;

            try
            {
                var supportsUri = await Xamarin.Essentials.Launcher.CanOpenAsync("fb://");
                if (supportsUri)
                {
                    var newsId = selectedNews.Id.Remove(0, _avilaFacebookPageId.Length + 1);
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var fbUrl = string.Format("fb://profile/{0}/posts?id={1}", _avilaFacebookPageId, newsId);
                        //await Launcher.OpenAsync("fb://profile/115592608462989/posts?id=" + selectedNews.Id);
                        await Launcher.OpenAsync(fbUrl);
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        await Launcher.OpenAsync("fb://facewebmodal/f?href=" + selectedNews.Url);
                    }
                }
                else
                {
                    await Browser.OpenAsync(selectedNews.Url);
                }
            }
            catch (Exception)
            {
                await Browser.OpenAsync(selectedNews.Url);
            }

        }

        private async Task OpenEventAsync(Event selectedEvent)
        {
            // event => OK / Android
            //await Xamarin.Essentials.Launcher.OpenAsync("fb://event/2720519278225723");
            // event => OK / iOS
            //await Xamarin.Essentials.Launcher.OpenAsync("fb://event?id=2720519278225723");

            if (selectedEvent == null)
                return;

            try
            {
                var supportsUri = await Xamarin.Essentials.Launcher.CanOpenAsync("fb://");
                if (supportsUri)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Launcher.OpenAsync("fb://event?id=" + selectedEvent.Id);
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        await Launcher.OpenAsync("fb://event/" + selectedEvent.Id);
                    }
                }
                else
                {
                    await Browser.OpenAsync(selectedEvent.Url);
                }
            }
            catch (Exception)
            {
                await Browser.OpenAsync(selectedEvent.Url);
            }
        }
    }
}

