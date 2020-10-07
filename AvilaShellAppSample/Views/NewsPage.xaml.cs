using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace AvilaShellAppSample.Views
{
    public partial class NewsPage : ContentPage
    {
        public NewsPage()
        {
            InitializeComponent();
        }

        void Image_SizeChanged(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Image_SizeChanged");
            var screenWidth = Application.Current.MainPage.Width;

            var image = sender as Image;
            var w = image.Width;
            var h = image.Height;
            
            if (h > 1 && w != screenWidth)
            {
                var ratio = w / h;

                image.WidthRequest = screenWidth;
                image.HeightRequest = screenWidth / ratio;
                //InvalidateMeasure();
            }

        }
    }
}
