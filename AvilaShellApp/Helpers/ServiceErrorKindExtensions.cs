using AvilaShellApp.Monitoring;
using AvilaShellApp.Services;

namespace AvilaShellApp.Helpers
{
    public static class ServiceErrorKindExtensions
    {

        public static string ToTitle(this ServiceErrorKind serviceErrorKind)
        {
            switch (serviceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    return Strings.Strings.ServiceErrorKindNoInternetAccessTitle;
                case ServiceErrorKind.ServiceIssue:
                case ServiceErrorKind.Timeout:
                    return Strings.Strings.ServiceErrorKindErrorAccessTitle;
                case ServiceErrorKind.None:
                default:
                    return string.Empty;
            }
        }

        public static string ToMessage(this ServiceErrorKind serviceErrorKind)
        {
            switch (serviceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    return Strings.Strings.ServiceErrorKindNoInternetAccessMessage;
                case ServiceErrorKind.ServiceIssue:
                case ServiceErrorKind.Timeout:
                    return Strings.Strings.ServiceErrorKindErrorAccessMessage;
                case ServiceErrorKind.None:
                default:
                    return string.Empty;
            }
        }

        public static string ToEventName(this ServiceErrorKind serviceErrorKind)
        {
            switch (serviceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    return EventName.NoInternetAccessRetry;
                case ServiceErrorKind.ServiceIssue:
                    return EventName.ServiceIssueRetry;
                case ServiceErrorKind.Timeout:
                    return EventName.TimeoutRetry;
                case ServiceErrorKind.None:
                default:
                    return string.Empty;
            }
        }

        public static string ToNewsServiceErrorPage(this ServiceErrorKind serviceErrorKind)
        {
            switch (serviceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    return EventPage.NewsPageNoInternetAccess;
                case ServiceErrorKind.ServiceIssue:
                    return EventPage.NewsPageServiceIssue;
                case ServiceErrorKind.Timeout:
                    return EventPage.NewsPageTimeoutError;
                case ServiceErrorKind.None:
                default:
                    return null;
            }
        }

        public static string ToBookingWebviewErrorPage(this ServiceErrorKind serviceErrorKind)
        {
            switch (serviceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    return EventPage.BookingPageNoInternetAccess;
                case ServiceErrorKind.ServiceIssue:
                    return EventPage.BookingPageServiceIssue;
                case ServiceErrorKind.Timeout:
                    return EventPage.BookingPageTimeoutError;
                case ServiceErrorKind.None:
                default:
                    return null;
            }
        }
    }
}
