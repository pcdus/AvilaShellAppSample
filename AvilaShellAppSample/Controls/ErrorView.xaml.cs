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

        public ErrorView()
        {
            InitializeComponent();
        }
    }
}
