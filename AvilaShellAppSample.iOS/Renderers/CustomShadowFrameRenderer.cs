using System;
using AvilaShellAppSample.Controls;
using AvilaShellAppSample.iOS.Renderers;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomShadowFrame), typeof(CustomShadowFrameRenderer))]
namespace AvilaShellAppSample.iOS.Renderers
{

    /// <summary>
    /// Customize the shadow effects of the Frame control in iOS to make the shadow effects looks similar to Android
    /// </summary>
    public class CustomShadowFrameRenderer : FrameRenderer
    {
        nfloat layerCornerRadius;
        float layerShadowOpacity;
        CGSize layerShadowOffset;
        CGRect layerBounds;
        CGColor layerBorderColor;
        nfloat layerBorderWidth;

        bool isSet = false;

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            /*
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
            */

            if (isSet)
            {
                Layer.ShadowOffset = layerShadowOffset;
                Layer.ShadowOpacity = layerShadowOpacity;
                Layer.BorderWidth = layerBorderWidth;
                Layer.BorderColor = layerBorderColor;
                Layer.CornerRadius = layerCornerRadius;
                Layer.Bounds = layerBounds;
            }


        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> element)
        {
            base.OnElementChanged(element);
            var customShadowFrame = (CustomShadowFrame)this.Element;
            if (customShadowFrame != null)
            {
                this.Layer.CornerRadius = customShadowFrame.Radius;
                this.Layer.ShadowOpacity = customShadowFrame.ShadowOpacity;
                this.Layer.ShadowOffset = new CGSize(customShadowFrame.ShadowOffsetWidth, customShadowFrame.ShadowOffSetHeight);
                this.Layer.Bounds.Inset(customShadowFrame.BorderWidth, customShadowFrame.BorderWidth);
                this.Layer.BorderColor = customShadowFrame.CustomBorderColor.ToCGColor();
                this.Layer.BorderWidth = (float)customShadowFrame.BorderWidth;

                layerCornerRadius = this.Layer.CornerRadius;
                layerShadowOpacity = this.Layer.ShadowOpacity;
                layerShadowOffset = this.Layer.ShadowOffset;
                layerBounds = this.Layer.Bounds;
                layerBorderColor = this.Layer.BorderColor;
                layerBorderWidth = this.Layer.BorderWidth;

                isSet = true;
            }
        }
    }
}
