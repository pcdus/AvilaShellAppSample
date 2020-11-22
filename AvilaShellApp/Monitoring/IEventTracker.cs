using System;
using System.Collections.Generic;

namespace AvilaShellApp.Monitoring
{
    public interface IEventTracker
    {
        void SetUserId(string userId);
        void Click(string name, string page, string destination, ICollection<KeyValuePair<string, string>> optionalParams = null);
        void Click(string element, ICollection<KeyValuePair<string, string>> optionalParams = null);
        void Display(string page, ICollection<KeyValuePair<string, string>> optionalParams = null);
        void UnknownData(string unknownValue, string valueKind, ICollection<KeyValuePair<string, string>> optionalParams = null);
        void PullToRefresh(string page);
        void Api(string apiType, long responseTime, ICollection<KeyValuePair<string, string>> optionalParams = null);
        void Error(Exception exception, ICollection<KeyValuePair<string, string>> optionalParams = null);
        void Information(string message, ICollection<KeyValuePair<string, string>> optionalParams = null);
        void Diagnostic(string tag, string data, ICollection<KeyValuePair<string, string>> optionalParams = null);
    }
}
