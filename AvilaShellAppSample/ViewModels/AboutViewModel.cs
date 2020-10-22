using System;
using System.Windows.Input;
using AvilaShellAppSample.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AvilaShellAppSample.ViewModels
{
    public class AboutViewModel : AvilaViewModelBase
    {
        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
        }

    }
}