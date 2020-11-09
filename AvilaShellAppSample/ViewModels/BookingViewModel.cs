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
using Lottie.Forms;

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

        bool showWebView = false;
        public bool ShowWebView
        {
            get { return showWebView; }
            set { SetProperty(ref showWebView, value); }
        }

        public bool IsConnected { get; set; }

        public MvvmHelpers.Commands.Command RefreshCommand => new MvvmHelpers.Commands.Command(this.Refresh);
        public ICommand RetryCommand => new Xamarin.Forms.Command<object>(Retry);

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
            ShowLoadingView = true;
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
                    break;
                case WebNavigationResult.Timeout:
                    Debug.WriteLine($"BookingViewModel - WebViewNavigatedAsync() - Timeout");
                    ErrorKind = ServiceErrorKind.Timeout;
                    break;
            }

            IsBusy = false;
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
            ShowLoadingView = true;
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
            return Task.CompletedTask;
        }

        #endregion

        #region Lottie Behaviors Command

        public ICommand OnFinishedAnimationCommand
        {
            get
            {
                return new Xamarin.Forms.Command<object>(async (object sender) =>
                {
                    if (sender != null)
                    {
                        await OnFinishedAnimation(sender);
                    }
                });
            }
        }

        private Task OnFinishedAnimation(object sender)
        {
            Debug.WriteLine($"BookingViewModel - OnFinishedAnimation()");

            var view = sender as AnimationView;
            if (IsBusy)
            {
                Debug.WriteLine($"BookingViewModel - OnFinishedAnimation() - animation replayed");
                view.PlayAnimation();
            }
            else
            {
                Debug.WriteLine($"BookingViewModel - OnFinishedAnimation() - animation ended");                
                ShowLoadingView = false;

                if (ErrorKind == ServiceErrorKind.None)
                    ShowWebView = true;
                else
                    SetErrorView();
            }
            return Task.CompletedTask;
        }

        #endregion

        public BookingViewModel()
        {
            Debug.WriteLine($"BookingViewModel - Ctor()");
            _eventTracker = new AppCenterEventTracker();

            Title = "Booking";

            _eventTracker.Display(EventPage.BookingPage);
        }

        private void SetErrorView()
        {
            Debug.WriteLine($"BookingViewModel - SetErrorView()");

            if (ErrorKind == ServiceErrorKind.None)
                return;

            var eventPage = ErrorKind.ToBookingWebviewErrorPage();
            _eventTracker.Display(eventPage);

            ErrorTitle = ErrorKind.ToTitle();
            ErrorDescription = ErrorKind.ToMessage();

            ShowErrorView = true;
        }

        #region User's interactions

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
                ReloadWebview(webView, true);
            }
            catch (Exception ex)
            {
                _eventTracker.Error(ex);
            }
        }

        private void ReloadWebview(CustomWebView webView, bool isRetry = false)
        {
            Debug.WriteLine($"BookingViewModel - ReloadWebview()");

            ErrorKind = ServiceErrorKind.None;
            ShowWebView = false;

            webView.Source = AvilaUrlBooking;
            webView.Uri = AvilaUrlBooking;

            webView.Reload();
            if (isRetry)
                webView.RetryNavigation();
            else
                webView.Refresh();
        }

        #endregion

    }
}
