using System;
using System.Threading.Tasks;

namespace AvilaShellApp.Infrastructure
{

    public interface IDeepLinkingLauncher
    {
        Task OpenFacebookPageAsync(string uri, string pageId);
        Task OpenFacebookPostAsync(string uri, string pageId, string postId);
        Task OpenFacebookEventAsync(string uri, string eventId);
    }
}
