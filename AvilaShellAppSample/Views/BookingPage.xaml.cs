using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AvilaShellAppSample.Views
{
    public partial class BookingPage : ContentPage
    {

        public BookingPage()
        {
            InitializeComponent();
            //webView.Source = "https://booking.wavy.pro/avila";
        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            this.webView.Reload();
        }

    }
}
