using System;
using Xamarin.Forms;

namespace AvilaShellAppSample.Controls
{
    public class NoShadowFrame : Frame
    {
        public NoShadowFrame()
        {
            CornerRadius = 4;
            HasShadow = false;
            Padding = 0;
            Margin = 0;

            // speical case for Android
            if (Device.RuntimePlatform == Device.Android)
            {
                this.BackgroundColor = Color.White;
                this.BorderColor = Color.FromHex("#ebecef");
            }
        }
    }
}
