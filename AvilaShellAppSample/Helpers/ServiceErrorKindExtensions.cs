using System;
using AvilaShellAppSample.Monitoring;
using AvilaShellAppSample.Services;

namespace AvilaShellAppSample.Helpers
{
    public static class ServiceErrorKindExtensions
    {

        public static string ToTitle(this ServiceErrorKind serviceErrorKind)
        {
            switch (serviceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    return "Pas de connexion Internet";
                case ServiceErrorKind.ServiceIssue:
                case ServiceErrorKind.Timeout:
                    return "Une erreur s'est produite";
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
                    return "Aucune connexion internet n'est disponible. Veuillez vérifier et réessayer.";
                case ServiceErrorKind.ServiceIssue:
                case ServiceErrorKind.Timeout:
                    return "Le service semble ne pas être accessible actuellement. Veuillez réessayer plus tard.";
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
