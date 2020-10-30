using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AvilaShellAppSample.Infrastructure
{
    public interface IMap
    {
        Task OpenAsync(Placemark placemark, MapLaunchOptions options);
    }
}
