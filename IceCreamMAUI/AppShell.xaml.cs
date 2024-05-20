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
    }
}
