using System;
namespace AvilaShellAppSample.Monitoring
{
    public static class EventType
    {
        public const string Click = nameof(Click);
        public const string Display = nameof(Display);
        public const string PullToRefresh = nameof(PullToRefresh);
        public const string Api = nameof(Api);
        public const string UnknownData = nameof(UnknownData);
        public const string Information = nameof(Information);
        public const string Diagnostic = nameof(Diagnostic);
    }
}
