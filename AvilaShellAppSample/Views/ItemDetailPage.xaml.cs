using System.ComponentModel;
using Xamarin.Forms;
using AvilaShellAppSample.ViewModels;

namespace AvilaShellAppSample.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}