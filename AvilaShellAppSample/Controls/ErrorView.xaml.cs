using System.Diagnostics;
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
                Debug.WriteLine($"ErrorView - ErrorKind getter : {(ServiceErrorKind)GetValue(ErrorKindProperty)}");
                var newValue = (ServiceErrorKind)GetValue(ErrorKindProperty);
                RelaunchAnimation(newValue);
                return (ServiceErrorKind)GetValue(ErrorKindProperty);
            }
            set
            {
                Debug.WriteLine($"ErrorView - ErrorKind setter : {value}");
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

        private void RelaunchAnimation(ServiceErrorKind errorKind)
        {
            if (errorKind == ServiceErrorKind.NoInternetAccess)
                networkErrorAnimationView.PlayAnimation();
            if (errorKind == ServiceErrorKind.ServiceIssue || errorKind == ServiceErrorKind.Timeout)
                serviceErrorAnimationView.PlayAnimation();
        }

        void NetworkErrorAnimationView_Clicked(System.Object sender, System.EventArgs e)
        {
            if (!networkErrorAnimationView.IsAnimating)
                networkErrorAnimationView.PlayAnimation();
        }

        void ServiceErrorAnimationView_Clicked(System.Object sender, System.EventArgs e)
        {
            if (!serviceErrorAnimationView.IsAnimating)
                serviceErrorAnimationView.PlayAnimation();
        }

    }
}
