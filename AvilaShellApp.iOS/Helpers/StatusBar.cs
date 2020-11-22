using System;
using AvilaShellApp.Infrastructure;
using AvilaShellApp.iOS;
using AvilaShellApp.iOS.Helpers;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBar))]
namespace AvilaShellApp.iOS.Helpers
{
    class StatusBar : IStatusBar
    {
        public double GetStatusBarHeight()
        {
            return (double)UIApplication.SharedApplication.StatusBarFrame.Height;
        }
    }
}
