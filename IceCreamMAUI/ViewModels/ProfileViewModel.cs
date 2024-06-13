using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        public ProfileViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty,NotifyPropertyChangedFor(nameof(Initials))]
        private string _name = "";

        public string Initials
        {
            get
            {
                var parts = Name.Split(' ',StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if(parts.Length > 1)
                {
                    return $"{parts[0][0]}{parts[1][0]}".ToUpper();
                }
                return Name.Length > 1 ? Name[..1].ToUpper() : Name.ToUpper();
            }
        }

        public void Initialize()
        {
            Name = _authService.User!.Name;
        }

        [RelayCommand]
        private async Task SignoutAsync()
        {
            _authService.Signout();
            await GoToAsync($"//{nameof(OnboardingPage)}");
        }

        [RelayCommand]
        private async Task GoToMyOrdersAsync()=>await GoToAsync(nameof(MyOrdersPage),animate:true);

    }
}
