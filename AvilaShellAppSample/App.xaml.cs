using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AvilaShellAppSample.Services;
using AvilaShellAppSample.Views;
using Xamarin.Forms.Svg;
using FFImageLoading.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace AvilaShellAppSample
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = true;

        public App()
        {
            //Register Syncfusion license: no longer used
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzMxMTM2QDMxMzgyZTMzMmUzMEZXOG80REI3eHlHcGREcE5jSFMxalFCYSswZXlIalNQNTNta3J0U2ZnbE09");

            InitializeComponent();

            // XF SvgImageSource registration
            SvgImageSource.RegisterAssembly();

            //UseMockDataStore = false;
            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // AppCenter
            AppCenter.Start("ios=9badc09d-7c2a-46d1-9a38-7698f5ad85c4;" +
                  "android=e1bfa407-e1e4-4cb1-bb70-698343265c67;",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
