using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AvilaShellApp.Infrastructure
{
    public interface IContact
    {
        void Call(string phoneNumber);
        Task SendEmailAsync(string email);
    }
}
