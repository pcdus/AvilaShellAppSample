using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AvilaShellAppSample.Infrastructure
{
    public interface IContact
    {
        void Call(string phoneNumber);
        Task SendEmailAsync(string email);
    }
}
