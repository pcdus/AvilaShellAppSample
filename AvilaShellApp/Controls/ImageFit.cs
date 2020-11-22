using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace AvilaShellApp.Controls
{
    /// <summary>
    /// Use to adjust size of images in news page
    /// </summary>
    public class ImageFit : CachedImage
    {

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            SizeRequest sizeRequest = base.OnMeasure(double.PositiveInfinity, double.PositiveInfinity);

            var innerRatio = sizeRequest.Request.Width / sizeRequest.Request.Height;

            if (double.IsNaN(innerRatio))
                return sizeRequest;

            // Width needs to be adjusted
            if (double.IsInfinity(heightConstraint))
            {
                // Height needs to be adjusted
                if (double.IsInfinity(widthConstraint))
                {
                    widthConstraint = sizeRequest.Request.Width;
                    heightConstraint = sizeRequest.Request.Height;
                }
                else
                {
                    // Adjust height
                    heightConstraint = widthConstraint * sizeRequest.Request.Height / sizeRequest.Request.Width;
                }
            }
            else if (double.IsInfinity(widthConstraint))
            {
                // Adjust width
                widthConstraint = heightConstraint * sizeRequest.Request.Width / sizeRequest.Request.Height;
            }
            else
            {
                // strech the image to make it fit while conserving it's ratio
                var outerRatio = widthConstraint / heightConstraint;

                var ratioFactor = (innerRatio >= outerRatio) ?
                    (widthConstraint / sizeRequest.Request.Width) :
                    (heightConstraint / sizeRequest.Request.Height);

                widthConstraint = sizeRequest.Request.Width * ratioFactor;
                heightConstraint = sizeRequest.Request.Height * ratioFactor;
            }
            sizeRequest = new SizeRequest(new Size(widthConstraint, heightConstraint));
            return sizeRequest;
        }
    }
}
