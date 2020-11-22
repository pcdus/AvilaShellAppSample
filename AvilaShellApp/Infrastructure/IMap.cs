using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AvilaShellApp.Infrastructure
{
    public interface IMap
    {
        Task OpenAsync(Placemark placemark, MapLaunchOptions options);
    }
}
