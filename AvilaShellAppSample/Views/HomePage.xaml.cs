using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AvilaShellAppSample.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            /*
            var assembly = this.GetType().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }

            Image EmbeddedImage = new Image
            {
                Source = ImageSource.FromResource("AvilaShellAppSample.Resources.avila_indoor.jpg")
            };

            ImageSource AvilaIndoorImageSource = ImageSource.FromResource("AvilaShellAppSample.Resources.avila_indoor.jpg");
            avilaImage.Source = AvilaIndoorImageSource;
            */
        }
    }
}
