using System.ComponentModel;
using Xamarin.Forms;
using AvilaShellApp.ViewModels;

namespace AvilaShellApp.Views
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