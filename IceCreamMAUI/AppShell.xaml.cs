using IceCreamMAUI.Pages;

namespace IceCreamMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoute();
        }

        private readonly static Type[] _routePageTypes =
            [
                typeof(SigninPage),
                typeof(SignupPage),
            ];

        private static void RegisterRoute()
        {
            foreach(var pageType in _routePageTypes)
            {
                Routing.RegisterRoute(pageType.Name, pageType);
            }
        }

        private async void FlyoutFooter_Tapped(object sender, TappedEventArgs e)
        {
            //Add your github link
            await Launcher.OpenAsync("https://github.com/HMZ-rajput/IceCreamMAUI");
        }

        private async void SignoutMenuItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.DisplayAlert("Alert","signout","OK");
        }
    }
}
