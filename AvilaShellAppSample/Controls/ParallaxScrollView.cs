using System;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using AvilaShellAppSample.Infrastructure;
using System.Threading.Tasks;

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
            BindableProperty.Create(nameof(LogoHeaderView), typeof(View), typeof(View), null);

        public static readonly BindableProperty HiddenViewProrperty =
            BindableProperty.Create(nameof(HiddenView), typeof(View), typeof(BoxView), null);

        public static readonly BindableProperty ContentViewProperty =
            BindableProperty.Create(nameof(ContentView), typeof(View), typeof(View), null);

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

        public View ContentView
        {
            get => (View)GetValue(ContentViewProperty);
            set => SetValue(ContentViewProperty, value);
        }

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

        #region variables

        /// <summary>
        /// Gets or sets the height of the header view (= page header)
        /// </summary>
        private double height;

        private double logoHeight;

        private double mainViewHeight;

        private bool isFadeTo0Animated = false;
        private bool isFadeTo1Animated = false;
        private bool isMarginUpdated = false;

        private static double defaultLogoHeight = 90;
        private static double defaultHiddenHeight = 45;
        private static double defaultVisibleHeaderHeight = 220;

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

            if (!isMarginUpdated)
                UpdateMainContainerMargin();

            var statusBarHeight = DependencyService.Get<IStatusBar>().GetStatusBarHeight();

            if (y < 0)
            {
                // ParallaxHeaderView
                // ==================
                this.ParallaxHeaderView.Scale = 1;
                this.ParallaxHeaderView.TranslationY = y;

                // ratio calculation
                // =================
                // based on defaultHeaderHeight
                // ----------------------------
                // - ratio decrease while scrolling
                //var ratio = (float)((defaultHeaderHeight - ScrollY) / defaultHeaderHeight);
                // - ratio increase while scrolling
                //var ratio = (float)((defaultHeaderHeight + ScrollY) / defaultHeaderHeight);
                // based on defaultHiddenViewHeight
                // --------------------------------
                //var scale = (double)((defaultHiddenViewHeight - (ScrollY)) / defaultHiddenViewHeight);
                //var ratio = (double)((defaultHiddenViewHeight + (double)(y)) / defaultHiddenViewHeight);
                var ratio = (defaultHiddenHeight + y) / defaultHiddenHeight;
                ratio = ratio < 0 ? 0 : ratio;

                /*
                // debug apply ratio
                // =================
                // to LogoHeaderView
                // -----------------
                this.LogoHeaderView.Scale = ratio;
                this.LogoHeaderView.Opacity = ratio;
                // to HiddenView
                // -------------
                var newHiddenHeightRequest = defaultHiddenHeight * ratio;
                newHiddenHeightRequest = newHiddenHeightRequest < 0 ? 0 : newHiddenHeightRequest;
                this.HiddenView.HeightRequest = newHiddenHeightRequest;
                */


                // logo scale (based on ratio)
                // ==========
                // continuous variation (y = x )
                // --------------------
                var logoScale = ratio;
                // cos / sin variations (increase then decrease)
                // --------------------
                // y = (sin(6x)*-0.75) + 0.75)
                //var logoScale = (Math.Sin(6 * scale) * -0.75) + 0.75;
                // y = (cos(3x−2) * 1.8)
                //var logoScale = (Math.Cos(3 * scale - 2) * 1.8);
                // y = (cos(2x−1))(1.8)
                //var logoScale = (Math.Cos(2 * scale  -1) * 1.8);
                // y=(sin(3x−3))(−0.5)+1
                //var logoScale = Math.Sin(3 * scale - 3) * -0.5 + 1;
                // progressive variations (increase based on formula)
                // ----------------------
                // y=-8xxx+10xx-x
                //var logoScale = (-8 * scale * scale * scale) + (10 * scale * scale) - scale;
                // y = (5/3) - ((2/3) * x)
                //var logoScale = (double)((-2 * scale * scale) + (2 * scale) + 1);
                //logoScale = logoScale < 0 ? 0 : logoScale;
                // apply scale
                // -----------
                this.LogoHeaderView.Scale = logoScale;


                // logo opacity
                // ============
                // calculated value
                // ----------------
                // y = - (4/3)xxx + 3 xx - (2/3)x
                //logoOpacity = -((4d / 3d) * Math.Pow(scale, 3)) + (3 * Math.Pow(scale, 2)) - (2d / 3d) * scale;
                /*
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
                */
                // conditionnal value
                // ------------------
                if (logoScale < 0.5)
                {
                    AnimateHideLogo();
                } 
                else
                {
                    AnimateDisplayLogo();
                }



                // hidden view
                // ===========
                var newHiddenHeightRequest = defaultHiddenHeight * ratio;
                newHiddenHeightRequest = newHiddenHeightRequest < 0 ? 0 : newHiddenHeightRequest;
                this.HiddenView.HeightRequest = newHiddenHeightRequest;

                Debug.WriteLine("y<0 => " +
                    " ScrollY : " + ScrollY.ToString() +
                    " - y : " + y.ToString() +
                    " - height : " + height.ToString() +
                    " - ratio : " + ratio.ToString() +
                    " - logoScale : " + logoScale.ToString() +
                    //" - logoOpacity : " + logoOpacity.ToString() +
                    " - newHiddenHeightRequest : " + newHiddenHeightRequest.ToString());


            }
            else if (Device.RuntimePlatform == "iOS")
            {
                // ParallaxHeaderView
                // ==================
                var newHeight = height + (ScrollY * -1);
                var newScale = newHeight / height;
                var newTranslation = -(ScrollY / 2);
                this.ParallaxHeaderView.Scale = newScale;
                this.ParallaxHeaderView.TranslationY = newTranslation;

                // LogoHeaderView : scale + opacity
                // ==============
                this.LogoHeaderView.Scale = 1;
                //this.LogoHeaderView.Opacity = 1;
                AnimateDisplayLogo();

                // HiddenView
                // ==========
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
                // ParallaxHeaderView
                // ==================
                this.ParallaxHeaderView.Scale = 1;
                this.ParallaxHeaderView.TranslationY = 0;

                // LogoHeaderView : scale + opacity
                // ==============
                this.LogoHeaderView.Scale = 1;
                //this.LogoHeaderView.Opacity = 1;
                AnimateDisplayLogo();

                // HiddenView
                // ==========
                this.HiddenView.HeightRequest = defaultHiddenViewHeight;

                Debug.WriteLine("else => " +
                    " ScrollY : " + ScrollY.ToString() +
                    " - y : " + y.ToString());
            }

        }

        
        private void AnimateDisplayLogo()
        {
            if (this.LogoHeaderView.Opacity != 1 & !isFadeTo1Animated)
            {
                Debug.WriteLine($"FadeTo1");
                isFadeTo0Animated = true;
                Task.Run(async () =>
                {
                    await this.LogoHeaderView.FadeTo(1, 250);
                    isFadeTo0Animated = false;
                });
            }
        }

        private void AnimateHideLogo()
        {
            if (this.LogoHeaderView.Opacity != 0 && !isFadeTo0Animated)
            {
                Debug.WriteLine($"FadeTo0");
                isFadeTo1Animated = true;
                Task.Run(async () => {
                    await this.LogoHeaderView.FadeTo(0, 250);
                    isFadeTo1Animated = false;
                });
            }
        }

        private void UpdateMainContainerMargin()
        {
            Debug.WriteLine($"height : {height}");

            var contentViewHeight = this.ContentView.Height;
            Debug.WriteLine($"contentViewHeight : {contentViewHeight}");

            double newMargin = 0;
            if (Device.RuntimePlatform == "iOS")
            {
                // iPhone 14 Pro simulator - statusBarHeight : 47
                // iPhone XS - statusBarHeight : 44
                // iPod Touch simulator - statusBarHeight : 20
                var statusBarHeight = DependencyService.Get<IStatusBar>().GetStatusBarHeight();

                // iPhone 14 Pro simulator - height : 670 - contentViewHeight : 476.33
                // iPhone XS - height : 641 - contentViewHeight : 476.33
                // iPod Touch simulator - height : 455 - contentViewHeight : 477

                // iPhone 14 Pro simulator - newMargin : 225 => OK
                // iPhone 14 XS - newMargin : 192.66 => OK
                // iPod Touch simulator - newMargin : -18 => OK
                newMargin = height - contentViewHeight + statusBarHeight - 16;
            }
            else
            {
                // Pixel X9 Emulator - statusBarHeight : 84
                // Xiaomi - statusBarHeight : 81
                // Samsung Galaxy S7 - statusBarHeight : 72
                var statusBarHeight = DependencyService.Get<IStatusBar>().GetStatusBarHeight();

                // Pixel X9 Emulator - height : 547.42 - contentViewHeight : 497.57
                // Xiaomi - height : 597.66 - contentViewHeight : 497.66
                // Samsung Galaxy S7 - height : 504 - contentViewHeight : 497.66

                // Pixel X9 Emulator - newMargin : 33 => +/- 30 pixels empty before top
                //newMargin = height - contentViewHeight - 16;

                // Pixel X9 Emulator - newMargin : 117 => +/- 30 pixels too much
                //newMargin = height - contentViewHeight + statusBarHeight - 16;

                // Pixel X9 Emulator - newMargin : 75 => OK
                // Xiaomi - newMargin : 124 => OK
                // Samsung Galaxy S7 - newMargin : 30.33 => OK (4 pixels are missing)
                newMargin = height - contentViewHeight + (statusBarHeight/2) - 12;
            }

            newMargin = newMargin < 16 ? 16 : newMargin;
            Debug.WriteLine($"newMargin : {newMargin}");
            this.ContentView.Margin = new Thickness(16, 16, 16, newMargin);

            isMarginUpdated = true;
        }

        #endregion
    }
}
