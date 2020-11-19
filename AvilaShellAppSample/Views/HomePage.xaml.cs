using System;
using System.Collections.Generic;
using AvilaShellAppSample.Infrastructure;
using Xamarin.Forms;

namespace AvilaShellAppSample.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            // for debug : get resources files
            /*
            var assembly = this.GetType().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            */
        }

    }
}
