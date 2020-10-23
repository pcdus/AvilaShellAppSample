using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AvilaShellAppSample.Services;
using AvilaShellAppSample.Models;
using System.Diagnostics;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using AvilaShellAppSample.Services.Abstractions;
using System.IO;
using System.Net;
using Polly.Timeout;
using Acr.UserDialogs;
using System.Windows.Input;
using AvilaShellAppSample.Monitoring;
using AvilaShellAppSample.Helpers;
using System.Collections.Generic;

namespace AvilaShellAppSample.ViewModels
{
    public class NewsViewModel : AvilaViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IEventTracker _eventTracker;

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

        bool hasServiceError = false;
        public bool HasServiceError
        {
            get { return hasServiceError; }
            set { SetProperty(ref hasServiceError, value); }
        }

        ServiceErrorKind serviceErrorKind = ServiceErrorKind.None;
        public ServiceErrorKind ServiceErrorKind
        {
            get { return serviceErrorKind; }
            set { SetProperty(ref serviceErrorKind, value); }
        }

        string serviceErrorDescription;
        public string ServiceErrorDescription
        {
            get { return serviceErrorDescription; }
            set { SetProperty(ref serviceErrorDescription, value); }
        }

        bool showErrorView = false;
        public bool ShowErrorView
        {
            get { return showErrorView; }
            set { SetProperty(ref showErrorView, value); }
        }


        public AsyncCommand RefreshCommand => new AsyncCommand(this.RefreshAsync);
        public AsyncCommand<News> OpenNewsCommand => new AsyncCommand<News>(this.OpenNewsAsync);
        public AsyncCommand<Event> OpenEventCommand => new AsyncCommand<Event>(this.OpenEventAsync);
        public ICommand RetryCommand => new Xamarin.Forms.Command(async () => await RetryAsync());

        public NewsViewModel()
        {
            Debug.WriteLine("NewsViewModel - Ctor()");

            _dataService = new DataService();
            _eventTracker = new AppCenterEventTracker();

            Title = "News";
            News = new ObservableCollection<News>();

            Task.Run(async () => await GetNewsAsync());
            _eventTracker.Display(EventPage.NewsPage);
        }

        private async Task GetNewsAsync(bool forceRefresh = false)
        {
            Debug.WriteLine("NewsViewModel - GetNewsAsync()");
            try
            {
                ShowErrorView = false;
                IsBusy = true;

                /*
                var _events = await _dataService.GetEvents(forceRefresh);
                Events = new ObservableCollection<Event>(_events);
                var _news = await _dataService.GetNews(forceRefresh);
                News = new ObservableCollection<News>(_news);
                */

                var newsAndEvents = await _dataService.GetNewsAndEvents(forceRefresh);
                News = new ObservableCollection<News>(newsAndEvents.news);
                Events = new ObservableCollection<Event>(newsAndEvents.events);
            }
            catch (IOException ioEx)
            {
                Debug.WriteLine($"NewsViewModel - GetNewsAsync() - IOException : {ioEx.Message}");
                _eventTracker.Error(ioEx);
                ServiceErrorKind = ServiceErrorKind.NoInternetAccess;
                await SetServiceError();
            }
            catch (WebException wEx)
            {
                Debug.WriteLine($"NewsViewModel - GetNewsAsync() - WebException : {wEx.Message}");
                _eventTracker.Error(wEx);
                ServiceErrorKind = ServiceErrorKind.NoSuccessStatusCode;
                await SetServiceError();
            }
            catch (TimeoutRejectedException trEx)
            {
                Debug.WriteLine($"NewsViewModel - GetNewsAsync() - TimeoutRejectedException : {trEx.Message}");
                _eventTracker.Error(trEx);
                ServiceErrorKind = ServiceErrorKind.Timeout;
                await SetServiceError();
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
                Debug.WriteLine($"NewsViewModel - GetNewsAsync() - Exception : {ex.Message}");
                ServiceErrorKind = ServiceErrorKind.Other;
                await SetServiceError();
            }
            finally
            {
                Debug.WriteLine("NewsViewModel - GetNewsAsync() - Finally");
                if (News.Count > 0 || Events.Count > 0)
                    HasEmptyData = false;

                IsBusy = false;
            }
        }

        private async Task SetServiceError()
        {
            Debug.WriteLine("NewsViewModel - SetServiceError()");
            /*
            switch (ServiceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    ServiceErrorDescription = "Aucune connexion internet n'est disponible.";
                    break;
                case ServiceErrorKind.NoSuccessStatusCode:
                case ServiceErrorKind.Timeout:
                case ServiceErrorKind.Other:
                default:
                    ServiceErrorDescription = "Le service ne réponds pas : rééssayez plus tard.";
                    break;
            }
            */

            ServiceErrorDescription = ServiceErrorKind.ToMessage();

            if (IsRefreshing)
                IsRefreshing = false;

            var eventPage = ServiceErrorKind.ToNewsServiceErrorPage();
            _eventTracker.Display(eventPage);

            if (HasEmptyData)
            {
                ShowErrorView = true;
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(ServiceErrorDescription, "Attention", "OK");
            }
        }

        private async Task RefreshAsync()
        {
            Debug.WriteLine("NewsViewModel - RefreshAsync()");
            _eventTracker.PullToRefresh(EventPage.NewsPage);
            IsRefreshing = true;
            try
            {
                await GetNewsAsync(true);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
            }
            IsRefreshing = false;
        }

        private async Task RetryAsync()
        {
            Debug.WriteLine("NewsViewModel - RetryAsync()");
            var eventName = ServiceErrorKind.ToEventName();
            _eventTracker?.Click(eventName, EventPage.NewsPage, EventPage.NewsPage);
            try
            {
                await GetNewsAsync(false);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
            }
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

            _eventTracker.Click(EventName.OpenNews, EventPage.NewsPage, EventPage.Uri,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(EventProperty.Uri, selectedNews.Url)
                });

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

            _eventTracker.Click(EventName.OpenEvent, EventPage.NewsPage, EventPage.Uri,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(EventProperty.Uri, selectedEvent.Url)
                });
                
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

