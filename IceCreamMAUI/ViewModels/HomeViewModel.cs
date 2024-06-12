using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using IceCreamMAUI.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.ViewModels
{
    public partial class HomeViewModel(IIcecreamApi icecreamApi, AuthService authService):BaseViewModel
    {
        private readonly IIcecreamApi _icecreamApi = icecreamApi;
        private readonly AuthService _authService = authService;

        [ObservableProperty]
        private string _userName = string.Empty;

        [ObservableProperty]
        private IcecreamDto[] _icecreams = [];

        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            UserName = _authService.User!.Name;
            if(_isInitialized) return;

            IsBusy = true;
            try
            {
                //api call to get icecream
                Icecreams = await _icecreamApi.GetIcecreamsAsync();
                _isInitialized = true;
            }catch(Exception ex)
            {
                _isInitialized = false;
                await ShowAlertMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private async Task GoToDetailsPageAsync(IcecreamDto icecream)
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(DetailsViewModel.Icecream)] = icecream,
            };
            await GoToAsync(nameof(DetailsPage),animate:true,parameter);

        }
    }
}
