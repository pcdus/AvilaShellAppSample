using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using CoreGraphics;
using UIKit;
using AvilaShellAppSample.Controls;
using AvilaShellAppSample.Platform.ios;
using AvilaShellAppSample.Platform.ios.Renderers;
using Xamarin.Essentials;

[assembly: ExportRenderer(typeof(NoShadowFrame), typeof(NoShadowFrameRenderer))]
namespace AvilaShellAppSample.Platform.ios.Renderers
{
    public class NoShadowFrameRenderer : FrameRenderer
    {

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            // Update shadow to match better material design standards of elevation
            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.White.CGColor;
            Layer.ShadowOffset = new CGSize(0, 0);
            Layer.ShadowOpacity = 0.00f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
            // items
            Layer.BorderWidth = 1;
            var borderColor = ColorConverters.FromHex("#ebecef");
            Layer.BorderColor = borderColor.ToPlatformColor().CGColor;
            Layer.CornerRadius = 4.0f;

        }
    }
}
