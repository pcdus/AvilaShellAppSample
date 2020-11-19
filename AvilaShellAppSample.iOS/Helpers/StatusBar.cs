using System;
using AvilaShellAppSample.Infrastructure;
using AvilaShellAppSample.iOS.Helpers;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBar))]
namespace AvilaShellAppSample.iOS.Helpers
{
    class StatusBar : IStatusBar
    {
        public double GetStatusBarHeight()
        {
            return (double)UIApplication.SharedApplication.StatusBarFrame.Height;
        }
    }
}
