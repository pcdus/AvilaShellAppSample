using System;
using Xamarin.Forms;

namespace AvilaShellAppSample.Controls
{
    public class ShadowFrame : Frame
    {
        public ShadowFrame()
        {

            CornerRadius = 1;

            // speical case for Android
            if (Device.RuntimePlatform == Device.Android)
            {
                this.BackgroundColor = Color.White;
                this.BorderColor = Color.White;
            }
        }
    }
}
