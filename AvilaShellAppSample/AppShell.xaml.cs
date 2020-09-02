using System;
using System.Collections.Generic;
using AvilaShellAppSample.ViewModels;
using AvilaShellAppSample.Views;
using Xamarin.Forms;

namespace AvilaShellAppSample
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
