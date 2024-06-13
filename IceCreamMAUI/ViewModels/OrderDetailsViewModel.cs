using CommunityToolkit.Mvvm.ComponentModel;
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
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    public partial class OrderDetailsViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly IOrderApi _orderApi;

        public OrderDetailsViewModel(AuthService authService, IOrderApi orderApi)
        {
            _authService = authService;
            _orderApi = orderApi;
        }

        [ObservableProperty]
        private long _orderId;

        [ObservableProperty]
        private OrderItemDto[] _orderItems = [];

        [ObservableProperty]
        private string _title = "Order Items";

        partial void OnOrderIdChanged(long value)
        {
            Title = $"Order #{value}";
            LoadOrderItemsAsync(value);
        }

        private async Task LoadOrderItemsAsync(long orderId)
        {
            IsBusy = true;
            try
            {
                OrderItems = await _orderApi.GetOrderItemsAsync(orderId);
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
    }
}
