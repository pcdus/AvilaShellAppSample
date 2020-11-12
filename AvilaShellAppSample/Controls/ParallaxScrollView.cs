using System;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Diagnostics;

namespace AvilaShellAppSample.Controls
{
    /// <summary>
    /// The Parallax scroll view
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ParallaxScrollView : ScrollView
    {
        #region Bindable Properties

        /// <summary>
        /// Bindable property to set the parallx header view.
        /// </summary>
        public static readonly BindableProperty ParallaxHeaderViewProperty =
           BindableProperty.Create(nameof(ParallaxScrollView), typeof(View), typeof(ParallaxScrollView), null);

        public static readonly BindableProperty LogoHeaderViewProrperty =
            BindableProperty.Create(nameof(LogoHeaderView), typeof(View), typeof(SvgCachedImage), null);

        #endregion

        #region variables

        /// <summary>
        /// Gets or sets the height of the header view.
        /// </summary>
        private double height;


        private double logoHeight;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the parallx scroll view.
        /// </summary>
        public ParallaxScrollView()
        {
            Scrolled += (sender, e) => this.Parallax();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Parallx header view.
        /// </summary>
        public View ParallaxHeaderView
        {
            get => (View)GetValue(ParallaxHeaderViewProperty);
            set => SetValue(ParallaxHeaderViewProperty, value);
        }

        public View LogoHeaderView
        {
            get => (View)GetValue(LogoHeaderViewProrperty);
            set => SetValue(LogoHeaderViewProrperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when scroll the view.
        /// </summary>
        public void Parallax()
        {

            if (this.ParallaxHeaderView == null)
                return;

            if (height <= 0)
                height = this.ParallaxHeaderView.Height;

            if (logoHeight <= 0)
                logoHeight = this.LogoHeaderView.Height;

            var y = -(int)((float)ScrollY / 2.0f);

            if (y < 0)
            {
                this.ParallaxHeaderView.Scale = 1;
                this.ParallaxHeaderView.TranslationY = y;

                var newLogoHeight = logoHeight + ((ScrollY / 2) * -1);
                var newLogoScale = newLogoHeight / logoHeight;
                var newLogoTranslation = (ScrollY / 2);
                var newLogoOpacity = Math.Log(newLogoScale) + 1; 
                this.LogoHeaderView.Scale = newLogoScale;
                //this.LogoHeaderView.TranslationY = newLogoTranslation;
                this.LogoHeaderView.Opacity = newLogoOpacity;

                Debug.WriteLine("y<0 => " +
                    " y : " + y.ToString() +
                    " - height : " + height.ToString() +
                    " - logoHeight : " + logoHeight.ToString() +
                    " - newLogoHeight : " + newLogoHeight.ToString() +
                    " - newLogoScale : " + newLogoScale.ToString() +
                    " - newLogoOpacity : " + newLogoOpacity.ToString() +
                    " - PHV.Scale : 1 " +
                    " - PHV.TranslationY : " + y.ToString()); 


            }
            else if (Device.RuntimePlatform == "iOS")
            {
                var newHeight = height + (ScrollY * -1);
                var newScale = newHeight / height;
                var newTranslation = -(ScrollY / 2);
                this.ParallaxHeaderView.Scale = newScale;
                this.ParallaxHeaderView.TranslationY = newTranslation;

                LogoHeaderView.Scale = newScale;
                LogoHeaderView.Opacity = 1;
                //this.LogoHeaderView.RotateTo(newTranslation);

                Debug.WriteLine("iOS => " +
                    " y : " + y.ToString() +
                    " - height : " + height.ToString() +
                    " - logoHeight : " + logoHeight.ToString() +
                    " - newHeight : " + newHeight.ToString() +
                    " - PHV.Scale : " + newScale.ToString() +
                    " - PHV.TranslationY : " + newTranslation.ToString()) ;
            }
            else
            {
                this.ParallaxHeaderView.Scale = 1;
                this.ParallaxHeaderView.TranslationY = 0;

                Debug.WriteLine("else => " +
                    " y : " + y.ToString() +
                    " - height : " + height.ToString() +
                    " - logoHeight : " + logoHeight.ToString() +
                    " - PHV.Scale : 1 " +
                    " - PHV.TranslationY : 0");
            }

        }

        #endregion
    }
}
