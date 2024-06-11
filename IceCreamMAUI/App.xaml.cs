using IceCreamMAUI.Services;

namespace IceCreamMAUI
{
    public partial class App : Application
    {
        public App(AuthService authService)
        {
            InitializeComponent();
            authService.Initialize();

            MainPage = new AppShell(authService);
        }
    }
}
