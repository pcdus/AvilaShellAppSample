using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AvilaShellAppSample.Controls
{
    public class ImageRatioContainer : ContentView
    {
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * this.AspectRatio));
        }

        public static BindableProperty AspectRatioProperty = BindableProperty.Create(nameof(AspectRatio), typeof(double), typeof(ImageRatioContainer), (double)1);

        public double AspectRatio
        {
            get { return (double)this.GetValue(AspectRatioProperty); }
            set
            {
                this.SetValue(AspectRatioProperty, value);
            }
        }

        public static BindableProperty ImageToSizeProperty = BindableProperty.Create(nameof(ImageToSize), typeof(string), typeof(ImageRatioContainer), null, propertyChanged: OnImageToSizeChanged);

        private static async void OnImageToSizeChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var size = await DependencyService.Get<IImageSizer>().GetSizeForImage((string)newvalue);
            (bindable as ImageRatioContainer).AspectRatio = size.Height / size.Width;
        }

        public string ImageToSize
        {
            get { return (string)this.GetValue(ImageToSizeProperty); }
            set { this.SetValue(ImageToSizeProperty, value); }
        }
    }

    public interface IImageSizer
    {
        Task<Size> GetSizeForImage(string fileName);
    }
}
