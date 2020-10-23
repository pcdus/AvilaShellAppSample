using System;
namespace AvilaShellAppSample.Monitoring
{
    public static class EventPage
    {
        // common pages
        public const string NotTracked = nameof(NotTracked);
        public const string Root = nameof(Root);
        public const string Uri = nameof(Uri);
        // app pages
        public const string HomePage = nameof(HomePage);
        public const string NewsPage = nameof(NewsPage);
        public const string BookingPage = nameof(BookingPage);
        public const string AboutPage = nameof(AboutPage);
        // errors app pages
        public const string NewsPageNoInternetAccess = nameof(NewsPageNoInternetAccess);
        public const string NewsPageNoSuccessStatusCode = nameof(NewsPageNoSuccessStatusCode);
        public const string NewsPageOtherError = nameof(NewsPageOtherError);
        public const string NewsPageTimeoutError = nameof(NewsPageTimeoutError);
        // external native apps pages
        public const string NativeCallApp = nameof(NativeCallApp);
        public const string NativeMapApp = nameof(NativeMapApp);
        public const string NativeMailApp = nameof(NativeMailApp);
    }
}
