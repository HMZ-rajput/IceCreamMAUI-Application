using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;

namespace IceCreamMAUI
{
    public partial class AppShell : Shell
    {
        private readonly AuthService _authseService;
        public AppShell(AuthService authService)
        {
            InitializeComponent();
            RegisterRoute();
            _authseService = authService;
        }

        private readonly static Type[] _routePageTypes =
            [
                typeof(SigninPage),
                typeof(SignupPage),
                typeof(MyOrdersPage),
                typeof(OrderDetailsPage),
                typeof(DetailsPage),
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
            _authseService.Signout();
            await Shell.Current.GoToAsync($"//{nameof(OnboardingPage)}");
        }
    }
}
