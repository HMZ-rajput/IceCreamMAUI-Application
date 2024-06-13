using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Models;
using IceCreamMAUI.Services;
using IceCreamMAUI.Shared.Dtos;
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

        public CartViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
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
            if (CartItems.Count == 0)
            {
                await ShowAlertMessage("No items in cart.");
                return;
            }
            if(await ConfirmAsync("Clear Cart","Do you really want to remove all items?"))
            {
                await _databaseService.ClearCartAsync();
                CartItems.Clear();
                await ShowToastAsync("Cart cleared");
                NotifyCartCountChange();
            }
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
    }
}
