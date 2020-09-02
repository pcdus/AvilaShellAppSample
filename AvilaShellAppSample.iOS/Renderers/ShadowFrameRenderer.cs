using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using CoreGraphics;
using UIKit;
using AvilaShellAppSample.Controls;
using AvilaShellAppSample.Platform.ios;
using AvilaShellAppSample.Platform.ios.Renderers;

[assembly: ExportRenderer(typeof(ShadowFrame), typeof(ShadowFrameRenderer))]
namespace AvilaShellAppSample.Platform.ios.Renderers
{
    public class ShadowFrameRenderer : FrameRenderer
    {

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            // Update shadow to match better material design standards of elevation
            //Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = 0.80f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}
