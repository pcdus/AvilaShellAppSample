using System;
using System.Collections.Generic;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

namespace AvilaShellApp.Monitoring
{
    public class AppCenterEventTracker : IEventTracker
    {

        private static IDictionary<string, string> BuildParameters(ICollection<KeyValuePair<string, string>> mainParams,
            ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            if (optionalParams != null && optionalParams.Count > 0)
            {
                foreach (var param in optionalParams)
                {
                    var contains = false;
                    foreach (var mainParam in mainParams)
                    {
                        if (string.Equals(mainParam.Key, param.Key))
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (!contains)
                        mainParams.Add(new KeyValuePair<string, string>(param.Key, param.Value));
                }
            }

            var appCenterParams = new Dictionary<string, string>();
            foreach (var param in mainParams)
            {
                appCenterParams.Add(param.Key, param.Value);
            }

            return appCenterParams;
        }

        public virtual void Api(string apiType, long responseTime, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>>
            {
                    new KeyValuePair<string, string>(EventProperty.ApiType, apiType),
                    new KeyValuePair<string, string>(EventProperty.ResponseTime, responseTime.ToString()),
            };
            Analytics.TrackEvent(EventType.Api, BuildParameters(mainParams, optionalParams));
        }

        public virtual void Click(string name, string page, string destination, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>>{
                    new KeyValuePair<string, string>(EventProperty.Name, name),
                    new KeyValuePair<string, string>(EventProperty.Page, page),
                    new KeyValuePair<string, string>(EventProperty.Destination, destination),
            };
            Analytics.TrackEvent(EventType.Click, BuildParameters(mainParams, optionalParams));
        }

        public virtual void Click(string element, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(EventProperty.Element, element),
            };
            Analytics.TrackEvent(EventType.Click, BuildParameters(mainParams, optionalParams));
        }

        public virtual void Diagnostic(string tag, string data, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(EventProperty.Tag, tag),
                new KeyValuePair<string, string>(EventProperty.Data, data)
            };

            Analytics.TrackEvent(EventType.Diagnostic, BuildParameters(mainParams, optionalParams));
        }

        public virtual void Display(string page, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(EventProperty.Page, page),
            };
            Analytics.TrackEvent(EventType.Display, BuildParameters(mainParams, optionalParams));
        }

        public virtual void Error(Exception exception, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>>
            {
            };

            Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, BuildParameters(mainParams, optionalParams));
        }

        public virtual void Information(string message, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(EventProperty.Message, message)
            };

            Analytics.TrackEvent(EventType.Information, BuildParameters(mainParams, optionalParams));
        }

        public virtual void PullToRefresh(string page)
        {
            Analytics.TrackEvent(EventType.PullToRefresh, new Dictionary<string, string> {
                    { EventProperty.Page, page},
            });
        }

        public void SetUserId(string userId)
        {
            AppCenter.SetUserId(userId);
        }

        public virtual void UnknownData(string unknownValue, string valueKind, ICollection<KeyValuePair<string, string>> optionalParams = null)
        {
            var mainParams = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>(EventProperty.ApiDto, valueKind),
                new KeyValuePair<string, string>(EventProperty.Name, unknownValue)
            };
            Analytics.TrackEvent(EventType.UnknownData, BuildParameters(mainParams, optionalParams));
        }

    }
}
