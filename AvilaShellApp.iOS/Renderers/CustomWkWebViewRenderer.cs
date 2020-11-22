using System;
using System.Diagnostics;
using AvilaShellApp.Controls;
using AvilaShellApp.iOS.Renderers;
using Foundation;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWkWebViewRenderer))]
namespace AvilaShellApp.iOS.Renderers
{
    public class CustomWkWebViewRenderer : ViewRenderer<CustomWebView, WKWebView>
    {
        public CustomWkWebViewRenderer()
        {
            Debug.WriteLine($"CustomWkWebViewRenderer - Ctor");
        }

        WKWebView webView;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomWebView> e)
        {
            base.OnElementChanged(e);

            Debug.WriteLine($"CustomWkWebViewRenderer - OnElementChanged()");

            if (Control == null)
            {
                Debug.WriteLine($"CustomWkWebViewRenderer - OnElementChanged() - Control == null");

                webView = new WKWebView(Frame, new WKWebViewConfiguration()
                {
                    MediaPlaybackRequiresUserAction = false
                });
                webView.NavigationDelegate = new CustomWkWebViewNavigationDelegate(Element);
                SetNativeControl(webView);

                Element.OnRefresh += (sender, ea) => Refresh(sender);
                Element.OnRetryNavigation += (sender, ea) => Retry(sender);
            }

            if (e.NewElement != null)
            {
                Debug.WriteLine($"CustomWkWebViewRenderer - OnElementChanged() - e.NewElement != null");

                Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
                webView.NavigationDelegate = new CustomWkWebViewNavigationDelegate(Element);
                SetNativeControl(webView);
            }

        }

        private void Refresh(object sender)
        {
            Debug.WriteLine($"CustomWkWebViewRenderer - Refresh()");

            //webView.ReloadFromOrigin();
            Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
        }

        private void Retry(object sender)
        {
            Debug.WriteLine($"CustomWkWebViewRenderer - Retry()");

            /*
            var element = sender as CustomWebView;
            var url = (NavigationDelegate as WKNavigationDelegateCustom).NavigatingToURL;
            LoadRequest(new NSMutableUrlRequest(new NSUrl(url)) { Headers = GetHeaders(element) });
            */
            Control.LoadRequest(new NSUrlRequest(new NSUrl(Element.Uri)));
            webView.NavigationDelegate = new CustomWkWebViewNavigationDelegate(Element);
            SetNativeControl(webView);
        }
    }
}
