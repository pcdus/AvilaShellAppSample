using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using CoreGraphics;
using UIKit;
using AvilaShellAppSample.Controls;
using AvilaShellAppSample.Platform.ios;
using AvilaShellAppSample.Platform.ios.Renderers;

[assembly: ExportRenderer(typeof(LightShadowFrame), typeof(LightShadowFrameRenderer))]
namespace AvilaShellAppSample.Platform.ios.Renderers
{
    public class LightShadowFrameRenderer : FrameRenderer
    {

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            // Update shadow to match better material design standards of elevation
            //Layer.ShadowRadius = 4.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(1, 1);
            Layer.ShadowOpacity = 0.40f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}
