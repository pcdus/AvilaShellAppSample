using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AvilaShellAppSample.Services;
using Xamarin.Forms;

namespace AvilaShellAppSample.Controls
{
    public partial class ErrorView : ContentView
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ErrorView));
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(ErrorView));
        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        public static readonly BindableProperty ErrorKindProperty = BindableProperty.Create(nameof(ErrorKind), typeof(ServiceErrorKind), typeof(ErrorView));
        public ServiceErrorKind ErrorKind
        {
            get
            {
                return (ServiceErrorKind)GetValue(ErrorKindProperty);
            }
            set
            {
                SetValue(ErrorKindProperty, value);
            }
        }

        public static readonly BindableProperty RetryButtonCommandProperty = BindableProperty.Create(
            propertyName: nameof(RetryButtonCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(ErrorView),
            defaultValue: default(ICommand),
            propertyChanged: OnTapCommandPropertyChanged);

        public ICommand RetryButtonCommand
        {
            get => (ICommand)GetValue(RetryButtonCommandProperty);
            set => SetValue(RetryButtonCommandProperty, value);
        }

        static void OnTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorView errorView && newValue is ICommand command)
            {
                errorView.RetryButtonCommand = command;
            }
        }

        public static readonly BindableProperty RetryButtonCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(RetryButtonCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(ErrorView),
            null);

        public object RetryButtonCommandParameter
        {
            get { return GetValue(RetryButtonCommandParameterProperty); }
            set { SetValue(RetryButtonCommandParameterProperty, value); }
        }

        public ErrorView()
        {
            InitializeComponent();
        }

        private void NetworkErrorAnimationView_Clicked(System.Object sender, System.EventArgs e)
        {
            if (!networkErrorAnimationView.IsAnimating)
                networkErrorAnimationView.PlayAnimation();
        }

        private void ServiceErrorAnimationView_Clicked(System.Object sender, System.EventArgs e)
        {
            if (!serviceErrorAnimationView.IsAnimating)
                serviceErrorAnimationView.PlayAnimation();
        }

        // Hack to launch the animation when the ErrorView is really displayed in the parent view
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(IsVisible))
            {
                if (this.IsVisible)
                {
                    PlayAnimationOnDisplay();
                }
            }
        }

        private void PlayAnimationOnDisplay()
        {
            Debug.WriteLine($"ErrorView - InvokePlayAnimationOnDisplay()");
            if (ErrorKind != ServiceErrorKind.None)
            {
                if (ErrorKind == ServiceErrorKind.NoInternetAccess)
                {
                    networkErrorAnimationView?.PlayAnimation();
                }
                if (ErrorKind == ServiceErrorKind.ServiceIssue || ErrorKind == ServiceErrorKind.Timeout)
                {
                    serviceErrorAnimationView?.PlayAnimation();
                }
            }
        }

    }
}
