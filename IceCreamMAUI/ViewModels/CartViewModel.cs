using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Models;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using IceCreamMAUI.Shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.ViewModels
{
    public partial class CartViewModel : BaseViewModel
    {
        public ObservableCollection<CartItem> CartItems { get; set; } = [];

        public static int TotalCartCount { get; set; }
        public static event EventHandler<int>? TotalCartCountChanged;

        private readonly DatabaseService _databaseService;
        private readonly IOrderApi _orderApi;
        private readonly AuthService _authService;

        public CartViewModel(DatabaseService databaseService, IOrderApi orderApi, AuthService authService)
        {
            _databaseService = databaseService;
            _orderApi = orderApi;
            _authService = authService;
        }

        public async void AddItemToCart(IcecreamDto icecream, int quantity, string flavor, string topping)
        {
            //add another condtion to check different flavor of icecream
            var exisitingItem = CartItems.FirstOrDefault(ci => ci.IcecreamId == icecream.Id);
            if (exisitingItem is not null)
            {
                var dbCartItem = await _databaseService.GetCartItemAsync(exisitingItem.Id);
                if (quantity <= 0)
                {
                    //user wants to remove this item from cart
                    await _databaseService.DeleteCartItem(dbCartItem);
                    CartItems.Remove(exisitingItem);
                    await ShowToastAsync("Icecream Removed From Cart");
                }
                else
                {
                    dbCartItem.Quantity = quantity;
                    await _databaseService.UpdateCartItem(dbCartItem);

                    exisitingItem.Quantity = quantity;
                    await ShowToastAsync("Quantity Updated In Cart");
                }
            }
            else
            {
                if(quantity > 0)
                {
                    var cartItem = new CartItem
                    {
                        IcecreamId = icecream.Id,
                        Name = icecream.Name,
                        Price = icecream.Price,
                        Quantity = quantity,
                        FlavorName = flavor,
                        ToppingName = topping,
                    };
                    var entity = new Data.CartItemEntity(cartItem);
                    await _databaseService.AddCartItem(entity);

                    cartItem.Id = entity.Id;

                    CartItems.Add(cartItem);
                    await ShowToastAsync("Icecream Added To Cart");
                }
                else
                {
                    await ShowToastAsync("Select Quantity");
                }
            }
            NotifyCartCountChange();
        }

        private void NotifyCartCountChange()
        {
            TotalCartCount = CartItems.Sum(i => i.Quantity);
            TotalCartCountChanged?.Invoke(null, TotalCartCount);
        }

        public int GetItemCartCount(int icecreamId)
        {
            var existingItem = CartItems.FirstOrDefault(i=>i.IcecreamId == icecreamId);
            return existingItem?.Quantity ?? 0;
        }

        public async Task InitializeCartAsync()
        {
            await _databaseService.InitializeDb();

            var dbItems = await _databaseService.GetAllCartItemsAsync();

            foreach(var dbItem in dbItems)
            {
                CartItems.Add(dbItem.ToCartItemModel());
            }
            NotifyCartCountChange();
        }

        [RelayCommand]
        public async Task ClearCartAsync()
        {
            await ClearCartInternalAsync(fromPlaceOrder: false);
        }

        [RelayCommand]
        private async Task RemoveCartItemAsync(int cartItemId)
        {
            if (await ConfirmAsync("Remove Item", "Do you really want to remove this item?"))
            {
                var existingItem = CartItems.FirstOrDefault(i => i.Id == cartItemId);

                if (existingItem is null)
                    return;

                CartItems.Remove(existingItem);

                var dbCartItem = await _databaseService.GetCartItemAsync(cartItemId);

                if (dbCartItem is null)
                    return;

                await _databaseService.DeleteCartItem(dbCartItem);

                await ShowToastAsync("Item Removed");
                NotifyCartCountChange();
            }
        }

        private async Task ClearCartInternalAsync(bool fromPlaceOrder)
        {
            if (!fromPlaceOrder && CartItems.Count == 0)
            {
                await ShowAlertMessage("No items in cart.");
                return;
            }

            if (fromPlaceOrder ||  await ConfirmAsync("Clear Cart", "Do you really want to remove all items?"))
            {
                await _databaseService.ClearCartAsync();
                CartItems.Clear();

                if(!fromPlaceOrder)
                    await ShowToastAsync("Cart cleared");

                NotifyCartCountChange();
            }
        }

        [RelayCommand]
        private async Task PlaceOrderAsync()
        {
            if (CartItems.Count == 0)
            {
                await ShowAlertMessage("No items in cart. Add items in cart to place order.");
                return;
            }
            IsBusy = true;
            try
            {
                var order = new OrderDto(0, DateTime.Now, CartItems.Sum(i => i.TotalPrice));
                var orderItems = CartItems.Select(i => new OrderItemDto(0, i.IcecreamId, i.Name, i.Quantity, i.Price, i.FlavorName, i.ToppingName)).ToArray();
                var orderPlaceDto = new OrderPlaceDto(order, orderItems);

                var result = await _orderApi.PlaceOrderAsync(orderPlaceDto);
                if (!result.IsSuccess)
                {
                    await ShowAlertMessage(result.ErrorMessage);
                    return;
                }

                //if order place success
                await ShowToastAsync("Order Placed");
                await ClearCartInternalAsync(fromPlaceOrder: true);


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
