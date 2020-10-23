using System;
using AvilaShellAppSample.Monitoring;
using AvilaShellAppSample.Services;

namespace AvilaShellAppSample.Helpers
{
    public static class ServiceErrorKindExtensions
    {
        public static string ToMessage(this ServiceErrorKind serviceErrorKind)
        {
            switch (serviceErrorKind)
            {
                case ServiceErrorKind.NoInternetAccess:
                    return "Aucune connexion internet n'est disponible.";
                case ServiceErrorKind.NoSuccessStatusCode:
                case ServiceErrorKind.Other:
                case ServiceErrorKind.Timeout:
                    return "Le service ne réponds pas : rééssayez plus tard.";
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
                case ServiceErrorKind.NoSuccessStatusCode:
                    return EventName.NoSuccessStatusCodeRetry;
                case ServiceErrorKind.Other:
                    return EventName.OtherRetry;
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
                case ServiceErrorKind.NoSuccessStatusCode:
                    return EventPage.NewsPageNoSuccessStatusCode;
                case ServiceErrorKind.Other:
                    return EventPage.NewsPageOtherError;
                case ServiceErrorKind.Timeout:
                    return EventPage.NewsPageTimeoutError;
                case ServiceErrorKind.None:
                default:
                    return null;
            }
        }
    }
}
