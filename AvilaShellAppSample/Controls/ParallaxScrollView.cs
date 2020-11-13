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

        public static readonly BindableProperty HiddenViewProrperty =
            BindableProperty.Create(nameof(HiddenView), typeof(View), typeof(BoxView), null);

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

        public View HiddenView
        {
            get => (View)GetValue(HiddenViewProrperty);
            set => SetValue(HiddenViewProrperty, value);
        }


        #endregion

        #region Methods

        /// <summary>
        /// Invoked when scroll the view.
        /// </summary>
        public void Parallax()
        {

            var defaultHeaderHeight = 220; //specified in XAML
            var defaultHiddenViewHeight = 45; //specified in XAML

            if (this.ParallaxHeaderView == null)
                return;

            if (height <= 0)
                height = this.ParallaxHeaderView.Height;

            var y = -(int)((float)ScrollY / 2.0f);

            if (y < 0)
            {
                this.ParallaxHeaderView.Scale = 1;
                this.ParallaxHeaderView.TranslationY = y;

                var logoScale = (float)((defaultHeaderHeight - ScrollY) / defaultHeaderHeight);
                double logoOpacity = 0;
                if (Device.RuntimePlatform == "iOS")
                {
                    // y = log (x * x) + 1 => y 0 when x <= 0.3
                    logoOpacity = Math.Log10(logoScale * logoScale) + 1;
                }
                else
                {
                    // y = ln (x * x) + 1 => y 0 when x <= 0.6
                    logoOpacity = Math.Log(logoScale * logoScale) + 1;
                }          
                logoOpacity = logoOpacity < 0 ? 0 : logoOpacity;
                
                this.LogoHeaderView.Scale = logoScale;
                this.LogoHeaderView.Opacity = logoOpacity;


                //this.HiddenView.Scale = logoScale;
                var hiddenHeight = defaultHiddenViewHeight * logoOpacity;
                this.HiddenView.HeightRequest = hiddenHeight;

                Debug.WriteLine("y<0 => " +
                    " ScrollY : " + ScrollY.ToString() +
                    " - y : " + y.ToString() +
                    " - height : " + height.ToString() +
                    " - logoScale : " + logoScale.ToString() +
                    " - logoOpacity : " + logoOpacity.ToString() +
                    " - hiddenHeight : " + hiddenHeight.ToString()); 

            }
            else if (Device.RuntimePlatform == "iOS")
            {
                var newHeight = height + (ScrollY * -1);
                var newScale = newHeight / height;
                var newTranslation = -(ScrollY / 2);
                this.ParallaxHeaderView.Scale = newScale;
                this.ParallaxHeaderView.TranslationY = newTranslation;

                this.LogoHeaderView.Scale = 1;
                this.LogoHeaderView.Opacity = 1;
                this.HiddenView.HeightRequest = defaultHiddenViewHeight;

                Debug.WriteLine("iOS => " +
                    " ScrollY : " + ScrollY.ToString() +
                    " - y : " + y.ToString() +
                    " - height : " + height.ToString() +
                    " - newHeight : " + newHeight.ToString() +
                    " - PHV.Scale : " + newScale.ToString() +
                    " - PHV.TranslationY : " + newTranslation.ToString());
            }
            else
            {
                this.ParallaxHeaderView.Scale = 1;
                this.ParallaxHeaderView.TranslationY = 0;

                this.LogoHeaderView.Scale = 1;
                this.LogoHeaderView.Opacity = 1;
                this.HiddenView.HeightRequest = defaultHiddenViewHeight;

                Debug.WriteLine("else => " +
                    " ScrollY : " + ScrollY.ToString() +
                    " - y : " + y.ToString() +
                    " - height : " + height.ToString());
            }

        }

        #endregion
    }
}
