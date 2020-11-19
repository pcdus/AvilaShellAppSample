using System;
using Android.App;
using AvilaShellAppSample.Droid.Helpers;
using AvilaShellAppSample.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBar))]
namespace AvilaShellAppSample.Droid.Helpers
{

    class StatusBar : IStatusBar
    {
        public static Activity Activity { get; set; }

        public double GetStatusBarHeight()
        {
            int statusBarHeight = -1;
            int resourceId = Activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = Activity.Resources.GetDimensionPixelSize(resourceId);
            }
            return statusBarHeight;
        }

    }
}
