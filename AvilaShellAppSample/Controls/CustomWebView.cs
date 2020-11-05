using System;
using AvilaShellAppSample.Services;
using Xamarin.Forms;

namespace AvilaShellAppSample.Controls
{
    public class CustomWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(CustomWebView),
            defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
        
        public ServiceErrorKind ErrorKind { get; set; }

        public event EventHandler LoadingStart;
        public event EventHandler LoadingFinished;
        public event EventHandler LoadingFailed;

        public EventHandler OnRetryNavigation { get; set; }
        public EventHandler OnRefresh { get; set; }

        public void InvokeCompleted()
        {
            if (this.LoadingFinished != null)
            {
                ErrorKind = ServiceErrorKind.None;
                this.LoadingFinished.Invoke(this, null);
            }
        }

        public void InvokeStarted()
        {
            if (this.LoadingStart != null)
            {
                ErrorKind = ServiceErrorKind.None;
                this.LoadingStart.Invoke(this, null);
            }
        }

        public void InvokeFailed(ServiceErrorKind errorKind)
        {
            if (this.LoadingFailed != null)
            {
                ErrorKind = errorKind;
                this.LoadingFailed.Invoke(this, null);
            }
        }

        /// <summary>
        /// Reattempts navigation to the page that was last attempted to navigate to
        /// </summary>
        public void RetryNavigation()
        {
            OnRetryNavigation?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Refreshes the current page
        /// </summary>
        public void Refresh()
        {
            OnRefresh?.Invoke(this, new EventArgs());
        }

    }
}
