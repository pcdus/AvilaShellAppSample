using System;
using System.Collections.Generic;
using AvilaShellApp.ViewModels;
using AvilaShellApp.Views;
using Xamarin.Forms;

namespace AvilaShellApp
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
