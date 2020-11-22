using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvilaShellApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
