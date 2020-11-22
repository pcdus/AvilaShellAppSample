using System;
using System.Diagnostics;
using AvilaShellApp.Controls;
using AvilaShellApp.Services;
using Foundation;
using WebKit;

namespace AvilaShellApp.iOS.Renderers
{
    public class CustomWkWebViewNavigationDelegate : WKNavigationDelegate
    {
        private CustomWebView element;


        public CustomWkWebViewNavigationDelegate(CustomWebView element)
        {
            this.element = element;

            Debug.WriteLine($"CustomWkWebViewNavigationDelegate - Ctor");
        }

        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            Debug.WriteLine($"CustomWkWebViewNavigationDelegate - DidFinishNavigation");
            element.InvokeCompleted();
            //base.DidFinishNavigation(webView, navigation);
        }

        public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            Debug.WriteLine($"CustomWkWebViewNavigationDelegate - DidStartProvisionalNavigation");

            element.InvokeStarted();
            //base.DidStartProvisionalNavigation(webView, navigation);
        }

        public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            Debug.WriteLine($"CustomWkWebViewNavigationDelegate - DidFailNavigation - error : {error}");

            var errorKind = ServiceErrorKind.None;
            switch (error.Code)
            {
                case -1009: // no internet access
                    {
                        errorKind = ServiceErrorKind.NoInternetAccess;
                        break;
                    }
                case -1001: // timeout
                    {
                        errorKind = ServiceErrorKind.Timeout;
                        break;
                    }
                case -1003: // server cannot be found
                case -1100: // url not found on server
                default:
                    {
                        errorKind = ServiceErrorKind.ServiceIssue;
                        break;
                    }
            }
            element.InvokeFailed(errorKind);
            //base.DidFailNavigation(webView, navigation, error);
        }

        [Export("webView:didFailProvisionalNavigation:withError:")]
        public override void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            Debug.WriteLine($"CustomWkWebViewNavigationDelegate - DidFailProvisionalNavigation - error : {error}");

            var errorKind = ServiceErrorKind.None;
            switch (error.Code)
            {
                case -1009: // no internet access
                    {
                        errorKind = ServiceErrorKind.NoInternetAccess;
                        break;
                    }
                case -1001: // timeout
                    {
                        errorKind = ServiceErrorKind.Timeout;
                        break;
                    }
                case -1003: // server cannot be found
                case -1100: // url not found on server
                default:
                    {
                        errorKind = ServiceErrorKind.ServiceIssue;
                        break;
                    }
            }
            element.InvokeFailed(errorKind);
            //base.DidFailProvisionalNavigation(webView, navigation, error);
        }
    }
}
