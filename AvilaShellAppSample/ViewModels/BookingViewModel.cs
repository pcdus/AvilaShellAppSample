using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using AvilaShellAppSample.Controls;
using AvilaShellAppSample.Helpers;
using AvilaShellAppSample.Monitoring;
using AvilaShellAppSample.Services;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AvilaShellAppSample.ViewModels
{
    public class BookingViewModel : AvilaViewModelBase
    {

        // fields
        private readonly IEventTracker _eventTracker;

        // properties
        string avilaUrlBooking = "https://booking.wavy.pro/avila";
        public string AvilaUrlBooking
        {
            get { return avilaUrlBooking; }
            set { SetProperty(ref avilaUrlBooking, value); }
        }

        bool isFirstDisplay;
        public bool IsFirstDisplay
        {
            get { return isFirstDisplay; }
            set { SetProperty(ref isFirstDisplay, value); }
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

        public bool IsConnected { get; set; }

        public MvvmHelpers.Commands.Command RefreshCommand => new MvvmHelpers.Commands.Command(this.Refresh);
        public ICommand RetryCommand => new Xamarin.Forms.Command<object>(Retry);


        /*
        public AsyncCommand WebViewNavigatingCommand => new AsyncCommand(this.WebViewNavigating);
        public AsyncCommand WebViewNavigatedCommand => new AsyncCommand(this.WebViewNavigated);
        */

        /*
        private Xamarin.Forms.Command<WebNavigatingEventArgs> navigatingCommand;
        public Xamarin.Forms.Command<WebNavigatingEventArgs> NavigatingCommand
        {
            get
            {
                return navigatingCommand ?? (navigatingCommand = new Xamarin.Forms.Command<WebNavigatingEventArgs>(
                    (param) =>
                    {
                        if (param != null)
                        {

                        }
                    },
                    (param) => true
                    ));
            }
        }
        */

        /*
        public ICommand NavigatingCommand => new Xamarin.Forms.Command(async () => await NavigationAsync());
        private Task NavigationAsync()
        {

            return Task.CompletedTask;
        }


        public ICommand NavigatedCommand => new Xamarin.Forms.Command(async () => await NavigatedAsync());
        private Task NavigatedAsync()
        {
            return Task.CompletedTask;
        }
        */

        #region Xamarin.Forms WebView Behaviors Commands : NavigatingCommand / Navigated Command

        public ICommand NavigatingCommand
        {
            get
            {
                return new Xamarin.Forms.Command<WebNavigatingEventArgs>(async (x) =>
                {
                    if (x != null)
                    {
                        await WebViewNavigatingAsync(x);
                    }
                });
            }
        }

        private Task WebViewNavigatingAsync(WebNavigatingEventArgs eventArgs)
        {
            Debug.WriteLine($"BookingViewModel - WebViewNavigatingAsync()");

            IsBusy = true;
            ShowErrorView = false;

            return Task.CompletedTask;
        }

        public ICommand NavigatedCommand
        {
            get
            {
                return new Xamarin.Forms.Command<WebNavigatedEventArgs>(async (x) =>
                {
                    if (x != null)
                    {
                        await WebViewNavigatedAsync(x);
                    }
                });
            }
        }

        private Task WebViewNavigatedAsync(WebNavigatedEventArgs eventArgs)
        {
            Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync()");

            IsBusy = false;
            switch (eventArgs.Result)
            {
                // to check : Cancel => error or not?
                case WebNavigationResult.Cancel:
                    Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync() - Cancel");
                    ErrorKind = ServiceErrorKind.None;
                    break;
                case WebNavigationResult.Failure:
                default:
                    Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync() - Failure");
                    IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
                    if (IsConnected)
                    {
                        Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync() - Failure : Failure");
                        ErrorKind = ServiceErrorKind.ServiceIssue;
                    }
                    else
                    {
                        Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync() - Failure : NoInternetAccess");
                        ErrorKind = ServiceErrorKind.NoInternetAccess;
                    }
                    break;
                case WebNavigationResult.Success:
                    Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync() - Success");
                    ErrorKind = ServiceErrorKind.None;
                    IsFirstDisplay = false;
                    break;
                case WebNavigationResult.Timeout:
                    Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync() - Timeout");
                    ErrorKind = ServiceErrorKind.Timeout;
                    break;
            }
            SetServiceError();

            return Task.CompletedTask;
        }

        #endregion


        #region iOS platform specific WkWebView Behaviors Command : LoadingStartCommand / LoadingFinishedCommand / LoadingFailedCommand

        public ICommand LoadingStartCommand
        {
            get
            {
                return new Xamarin.Forms.Command(async () =>
                {
                    await WebViewLoadingStartAsync();
                });
            }
        }

        private Task WebViewLoadingStartAsync()
        {
            Debug.WriteLine($"BookingViewModel - WebViewLoadingStartAsync()");

            IsBusy = true;
            ShowErrorView = false;

            return Task.CompletedTask;
        }

        public ICommand LoadingFinishedCommand
        {
            get
            {
                return new Xamarin.Forms.Command(async () =>
                {
                    await WebViewLoadingFinishedAsync();
                });
            }
        }

        private Task WebViewLoadingFinishedAsync()
        {
            Debug.WriteLine($"BookingViewModel - WebViewLoadingFinishedAsync()");

            IsBusy = false;
            IsFirstDisplay = false;

            return Task.CompletedTask;
        }

        public ICommand LoadingFailedCommand
        {
            get
            {
                return new Xamarin.Forms.Command<object>(async (object sender) =>
                {
                    if (sender != null)
                    {
                        await WebViewLoadingFailedAsync(sender);
                    }
                });
            }
        }

        private Task WebViewLoadingFailedAsync(object sender)
        {
            Debug.WriteLine($"BookingViewModel - WebViewLoadingFailedAsync()");

            var view = sender as CustomWebView;
            ErrorKind = view.ErrorKind;
            Debug.WriteLine($"BookingViewModel - WebViewLoadingFailedAsync() - error : {ErrorKind}");

            IsBusy = false;
            SetServiceError();

            return Task.CompletedTask;
        }

        #endregion

        public BookingViewModel()
        {
            Debug.WriteLine($"BookingViewModel - Ctor()");

            _eventTracker = new AppCenterEventTracker();

            IsFirstDisplay = true;
            Title = "Booking";

            _eventTracker.Display(EventPage.BookingPage);
        }

        private void Refresh(object sender)
        {
            Debug.WriteLine($"BookingViewModel - Refresh()");
            _eventTracker?.Click(EventName.RefreshBookingWebview, EventPage.BookingPage, EventPage.BookingPage);

            try
            { 
                var webView = sender as CustomWebView;
                ReloadWebview(webView);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
            }
        }

        private void Retry(object sender)
        {
            Debug.WriteLine($"BookingViewModel - Retry()");
            var eventName = ErrorKind.ToEventName();
            _eventTracker?.Click(eventName, EventPage.BookingPage, EventPage.BookingPage);

            try
            {
                var webView = sender as CustomWebView;
                ReloadWebview(webView);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
            }
        }

        private void ReloadWebview(CustomWebView webView)
        {
            IsFirstDisplay = true;

            webView.Source = AvilaUrlBooking;
            webView.Uri = AvilaUrlBooking;

            webView.Reload();
            webView.Refresh();
        }

        private void SetServiceError()
        {
            Debug.WriteLine($"BookingViewModel - SetServiceError()");

            if (ErrorKind == ServiceErrorKind.None)
                return;

            var eventPage = ErrorKind.ToBookingWebviewErrorPage();
            _eventTracker.Display(eventPage);

            ErrorTitle = ErrorKind.ToTitle();
            ErrorDescription = ErrorKind.ToMessage();

            ShowErrorView = true;
        }

        /*
        private async Task WebViewNavigating()
        {
            //IsBusy = true;
            //await Task.Delay(1000);
        }

        private async Task WebViewNavigated()
        {
            //await Task.Delay(1000);
            //IsBusy = false;
            IsFirstDisplay = false;
        }
        */
    }
}
