using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace AvilaShellAppSample.Controls
{
    public class FullWidthCachedImage : CachedImage
    {
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var sr = base.OnMeasure(widthConstraint, heightConstraint);
            if (sr.Request.IsZero)
                return sr;

            var ratioWH = sr.Request.Width / sr.Request.Height;
            var sr2 = new SizeRequest(new Size(widthConstraint, widthConstraint / ratioWH));
            return sr2;
        }

    }
}