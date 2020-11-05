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
using AvilaShellAppSample.Infrastructure;

namespace AvilaShellAppSample.ViewModels
{
    public class NewsViewModel : AvilaViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IEventTracker _eventTracker;
        private readonly IDeepLinkingLauncher _deepLinkingLauncher;

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

        ServiceErrorKind errorKind = ServiceErrorKind.None;
        public ServiceErrorKind ErrorKind
        {
            get { return errorKind; }
            set { SetProperty(ref errorKind, value); }
        }

        string errorDescription;
        public string ErrorDescription
        {
            get { return errorDescription; }
            set { SetProperty(ref errorDescription, value); }
        }

        string errorTitle;
        public string ErrorTitle
        {
            get { return errorTitle; }
            set { SetProperty(ref errorTitle, value); }
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
            _deepLinkingLauncher = new DeepLinkingLauncher();

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
                ErrorKind = ServiceErrorKind.None;
                IsBusy = true;

                await Task.Delay(2000);
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
                ErrorKind = ServiceErrorKind.NoInternetAccess;
            }
            catch (WebException wEx)
            {
                Debug.WriteLine($"NewsViewModel - GetNewsAsync() - WebException : {wEx.Message}");
                _eventTracker.Error(wEx);
                ErrorKind = ServiceErrorKind.ServiceIssue;
            }
            catch (TimeoutRejectedException trEx)
            {
                Debug.WriteLine($"NewsViewModel - GetNewsAsync() - TimeoutRejectedException : {trEx.Message}");
                _eventTracker.Error(trEx);
                ErrorKind = ServiceErrorKind.Timeout;
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
                Debug.WriteLine($"NewsViewModel - GetNewsAsync() - Exception : {ex.Message}");
                ErrorKind = ServiceErrorKind.ServiceIssue;
            }
            finally
            {
                Debug.WriteLine("NewsViewModel - GetNewsAsync() - Finally");
                await SetServiceError();
                if (News.Count > 0 || Events.Count > 0)
                {
                    HasEmptyData = false;
                }
                IsBusy = false;
            }
        }

        private async Task SetServiceError()
        {
            Debug.WriteLine("NewsViewModel - SetServiceError()");

            if (ErrorKind == ServiceErrorKind.None)
                return;

            if (IsRefreshing)
                IsRefreshing = false;

            var eventPage = ErrorKind.ToNewsServiceErrorPage();
            _eventTracker.Display(eventPage);

            ErrorTitle = ErrorKind.ToTitle();
            ErrorDescription = ErrorKind.ToMessage();

            if (HasEmptyData)
            {
                ShowErrorView = true;
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(ErrorDescription, ErrorTitle, "OK");
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
            var eventName = ErrorKind.ToEventName();
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
            if (selectedNews == null)
                return;

            _eventTracker.Click(EventName.OpenNews, EventPage.NewsPage, EventPage.Uri,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(EventProperty.Uri, selectedNews.Url)
                });

            var fbPostId = selectedNews.Id.Remove(0, _avilaFacebookPageId.Length + 1);
            await _deepLinkingLauncher.OpenFacebookPostAsync(selectedNews.Url, _avilaFacebookPageId, fbPostId);
        }

        private async Task OpenEventAsync(Event selectedEvent)
        {
            if (selectedEvent == null)
                return;

            _eventTracker.Click(EventName.OpenEvent, EventPage.NewsPage, EventPage.Uri,
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(EventProperty.Uri, selectedEvent.Url)
                });

            await _deepLinkingLauncher.OpenFacebookEventAsync(selectedEvent.Url, selectedEvent.Id);
        }
    }
}

