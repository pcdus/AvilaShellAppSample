using System;
using System.Windows.Input;
using AvilaShellAppSample.Infrastructure;
using AvilaShellAppSample.Monitoring;
using AvilaShellAppSample.Services;
using MvvmHelpers.Commands;
using Command = MvvmHelpers.Commands.Command;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AvilaShellAppSample.ViewModels
{
    public class AboutViewModel : AvilaViewModelBase
    {
        private readonly IEventTracker _eventTracker;
        private readonly IContact _contact;
        private readonly IBrowser _browser;
        private readonly IDeepLinkingLauncher _deepLinkingLauncher;

        private readonly string hourrappsPhoneNumber = "0622107947";
        private readonly string hourrappsUrl = "http://hourrapps.com/";
        private readonly string hourrapsEmail = "contact@hourrapps.com";
        private readonly string hourrappsFbPage = "https://www.facebook.com/hourrapps";
        private readonly string hourrappsFbPageId = "1702965483338996";

        string avilaAppVersion;
        public string AvilaAppVersion
        {
            get { return avilaAppVersion; }
            set { SetProperty(ref avilaAppVersion, value); }
        }

        //public ICommand OpenWebCommand { get; }
        public Command CallHourrappsCommand => new Command(this.CallHourrapps);
        public AsyncCommand SendHourrappsEmailCommand => new AsyncCommand(this.SendHourrappsEmailAsync);
        public AsyncCommand OpenHourrappsWebsiteCommand => new AsyncCommand(this.OpenHourrappsWebsiteAsync);
        public AsyncCommand OpenHourrappsFacebookPageCommand => new AsyncCommand(this.OpenHourrappsFacebookPageAsync);

        public AboutViewModel()
        {
            _eventTracker = new AppCenterEventTracker();
            _contact = new Contact();
            _browser = new Infrastructure.Browser();
            _deepLinkingLauncher = new DeepLinkingLauncher();

            Title = "About";

            AvilaAppVersion = AppInfo.VersionString;

            _eventTracker.Display(EventPage.AboutPage);
        }

        private void CallHourrapps()
        {
            _eventTracker.Click(EventName.HourrappsCall, EventPage.AboutPage, EventPage.NativeCallApp);
            _contact.Call(hourrappsPhoneNumber);
        }

        private async Task SendHourrappsEmailAsync()
        {
            _eventTracker.Click(EventName.HourrappsMail, EventPage.AboutPage, EventPage.NativeMailApp);
            await _contact.SendEmailAsync(hourrapsEmail);
        }

        private async Task OpenHourrappsWebsiteAsync()
        {
            _eventTracker.Click(EventName.HourrappsWebsite, EventPage.AboutPage, EventPage.NativeMailApp);
            await _browser.OpenAsync(hourrappsUrl);
        }

        private async Task OpenHourrappsFacebookPageAsync()
        {
            _eventTracker.Click(EventName.HourrappsFacebookPage, EventPage.AboutPage, EventPage.NativeMailApp);
            await _deepLinkingLauncher.OpenFacebookPageAsync(hourrappsFbPage, hourrappsFbPageId);
        }
    }
}