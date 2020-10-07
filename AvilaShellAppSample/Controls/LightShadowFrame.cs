using System;
using Xamarin.Forms;

namespace AvilaShellAppSample.Controls
{
    public class LightShadowFrame : Frame
    {
        public LightShadowFrame()
        {
            CornerRadius = 4;
            HasShadow = true;
            //Padding = 0;
            //Margin = 0;

            // speical case for Android
            if (Device.RuntimePlatform == Device.Android)
            {
                this.BackgroundColor = Color.White;
                //this.BorderColor = Color.FromHex("#ebecef");
                this.BorderColor = Color.White;
            }
        }
    }
}
