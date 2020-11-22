using System;
using System.Threading.Tasks;
using AvilaShellApp.Services;
using AvilaShellApp.Models;
using System.Diagnostics;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using AvilaShellApp.Services.Abstractions;
using System.IO;
using System.Net;
using Polly.Timeout;
using Acr.UserDialogs;
using System.Windows.Input;
using AvilaShellApp.Monitoring;
using AvilaShellApp.Helpers;
using System.Collections.Generic;
using AvilaShellApp.Infrastructure;
using Lottie.Forms;

namespace AvilaShellApp.ViewModels
{
    public class NewsViewModel : AvilaViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IEventTracker _eventTracker;
        private readonly IDeepLinkingLauncher _deepLinkingLauncher;

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

        bool showLoadingView = false;
        public bool ShowLoadingView
        {
            get { return showLoadingView; }
            set { SetProperty(ref showLoadingView, value); }
        }

        public AsyncCommand RefreshCommand => new AsyncCommand(this.RefreshAsync);
        public AsyncCommand<News> OpenNewsCommand => new AsyncCommand<News>(this.OpenNewsAsync);
        public AsyncCommand<Event> OpenEventCommand => new AsyncCommand<Event>(this.OpenEventAsync);
        public ICommand RetryCommand => new Xamarin.Forms.Command(async () => await RetryAsync());

        #region Lottie Behaviors Command

        public ICommand OnFinishedAnimationCommand
        {
            get
            {
                return new Xamarin.Forms.Command<object>(async (object sender) =>
                {
                    if (sender != null)
                    {
                        await OnFinishedAnimationAsync(sender);
                    }
                });
            }
        }

        private async Task OnFinishedAnimationAsync(object sender)
        {
            Debug.WriteLine($"NewsViewModel - OnFinishedAnimation()");

            var view = sender as AnimationView;
            if (IsBusy)
            {
                Debug.WriteLine($"NewsViewModel - OnFinishedAnimation() - animation replayed");
                view.PlayAnimation();
            }
            else
            {
                Debug.WriteLine($"NewsViewModel - OnFinishedAnimation() - animation ended");
                ShowLoadingView = false;

                await SetErrorViewAsync();
            }
            //return Task.CompletedTask;
        }

        // Hack to abort the Animation when the Animation is playing when switching tab on iOS (called by the View)
        public void AbortAnimation(object sender)
        {
            Task.Run(async () => await OnFinishedAnimationAsync(sender));
        }

        #endregion

        public NewsViewModel()
        {
            Debug.WriteLine("NewsViewModel - Ctor()");

            _dataService = new DataService();
            _eventTracker = new AppCenterEventTracker();
            _deepLinkingLauncher = new DeepLinkingLauncher();

            Title = Strings.Strings.NewsPageTitle;

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

                ShowLoadingView = HasEmptyData;

                await Task.Delay(250);

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

                IsBusy = false;

                if (News.Count > 0 || Events.Count > 0)
                {
                    HasEmptyData = false;
                }

                if (IsRefreshing)
                    await SetErrorViewAsync();
            }
        }

        private async Task SetErrorViewAsync()
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

        #region User's interactions

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

            var fbPostId = selectedNews.Id.Remove(0, ApiConfig.FbAvilaPageId.Length + 1);
            await _deepLinkingLauncher.OpenFacebookPostAsync(selectedNews.Url, ApiConfig.FbAvilaPageId, fbPostId);
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

        #endregion

    }
}

