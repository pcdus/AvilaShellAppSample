using System;

using Xamarin.Forms;

namespace AvilaShellAppSample.ViewModels
{
    public class NewsViewModel : ContentPage
    {
        public NewsViewModel()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

