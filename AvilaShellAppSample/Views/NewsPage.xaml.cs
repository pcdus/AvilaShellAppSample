using System;
using System.Collections.Generic;
using System.Diagnostics;
using AvilaShellAppSample.ViewModels;
using Xamarin.Forms;

namespace AvilaShellAppSample.Views
{
    public partial class NewsPage : ContentPage
    {

        private readonly NewsViewModel _vm;

        public NewsPage()
        {
            InitializeComponent();

            _vm = this.BindingContext as NewsViewModel;
        }

        protected override void OnAppearing()
        {
            Debug.WriteLine($"NewsPage - OnAppearing - animation : {this.animationView.IsAnimating}");
            base.OnAppearing();

            // Hack to abort the Animation when the Animation is playing when switching tab on iOS (called by the View)
            if (_vm.ShowLoadingView)
                _vm?.AbortAnimation((object)this.animationView);
        }

        protected override void OnDisappearing()
        {
            Debug.WriteLine($"NewsPage - OnDisappearing - animation : {this.animationView.IsAnimating}");
            base.OnDisappearing();
        }
    }
}
