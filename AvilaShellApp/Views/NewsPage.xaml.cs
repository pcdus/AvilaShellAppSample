using System.Diagnostics;
using AvilaShellApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvilaShellApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
