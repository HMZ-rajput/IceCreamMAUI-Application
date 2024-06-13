using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using IceCreamMAUI.Shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.ViewModels
{
    public partial class OrdersViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly IOrderApi _orderApi;

        public OrdersViewModel(AuthService authService, IOrderApi orderApi)
        {
            _authService = authService;
            _orderApi = orderApi;
        }

        [ObservableProperty]
        private OrderDto[] _orders = [];

        public async Task InitializeAsync() => await LoadOrdersAsync();

        [RelayCommand]
        private async Task LoadOrdersAsync()
        {
            IsBusy = true;
            try
            {
                Orders = await _orderApi.GetMyOrdersAsync();
                if(Orders.Length== 0)
                {
                    await ShowToastAsync("No Orders Found");
                }
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await ShowAlertMessage("Session Expired");
                    _authService.Signout();
                    await GoToAsync($"//{nameof(OnboardingPage)}");
                    return;
                }
                await ShowAlertMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToOrderDetailsPageAsync(long orderId)
        {
            var parameter = new Dictionary<string, object>
            {
                [nameof(OrderDetailsViewModel.OrderId)] = orderId,
            };
            await GoToAsync(nameof(OrderDetailsPage), animate: true, parameter);
        }

    }
}
