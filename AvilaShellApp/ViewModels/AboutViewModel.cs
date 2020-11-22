using AvilaShellApp.Infrastructure;
using AvilaShellApp.Monitoring;
using AvilaShellApp.Services;
using MvvmHelpers.Commands;
using Command = MvvmHelpers.Commands.Command;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace AvilaShellApp.ViewModels
{
    public class AboutViewModel : AvilaViewModelBase
    {
        private readonly IEventTracker _eventTracker;
        private readonly IContact _contact;
        private readonly IBrowser _browser;
        private readonly IDeepLinkingLauncher _deepLinkingLauncher;

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

            Title = Strings.Strings.AboutPageTitle;
            AvilaAppVersion = AppInfo.VersionString;

            _eventTracker.Display(EventPage.AboutPage);
        }

        private void CallHourrapps()
        {
            _eventTracker.Click(EventName.HourrappsCall, EventPage.AboutPage, EventPage.NativeCallApp);
            _contact.Call(Strings.Strings.AboutPageContactHourrappsPhoneNumber);
        }

        private async Task SendHourrappsEmailAsync()
        {
            _eventTracker.Click(EventName.HourrappsMail, EventPage.AboutPage, EventPage.NativeMailApp);
            await _contact.SendEmailAsync(Strings.Strings.AboutPageContactHourrappsEmail);
        }

        private async Task OpenHourrappsWebsiteAsync()
        {
            _eventTracker.Click(EventName.HourrappsWebsite, EventPage.AboutPage, EventPage.NativeMailApp);
            await _browser.OpenAsync(Strings.Strings.AboutPageContactHourrappsWebsite);
        }

        private async Task OpenHourrappsFacebookPageAsync()
        {
            _eventTracker.Click(EventName.HourrappsFacebookPage, EventPage.AboutPage, EventPage.NativeMailApp);
            await _deepLinkingLauncher.OpenFacebookPageAsync(ApiConfig.FbHourrappsPageUrl, ApiConfig.FbHourrappsPageId);
        }
    }
}