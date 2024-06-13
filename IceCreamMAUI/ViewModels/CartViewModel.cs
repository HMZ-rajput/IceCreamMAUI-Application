using IceCreamMAUI.Models;
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

        public async void AddItemToCart(IcecreamDto icecream, int quantity, string flavor, string topping)
        {
            //add another condtion to check different flavor of icecream
            var exisitingItem = CartItems.FirstOrDefault(ci => ci.IcecreamId == icecream.Id);
            if(exisitingItem is not null)
            {
                if(quantity <= 0)
                {
                    //user wants to remove this item from cart
                    CartItems.Remove(exisitingItem);
                    await ShowToastAsync("Icecream Removed From Cart");
                }
                else
                {
                    exisitingItem.Quantity = quantity;
                    await ShowToastAsync("Quantity Updated In Cart");
                }
            }
            else
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
                CartItems.Add(cartItem);
                await ShowToastAsync("Icecream Added To Cart");
            }
            TotalCartCount = CartItems.Sum(i=>i.Quantity);
            TotalCartCountChanged?.Invoke(null, TotalCartCount);
        }


    }
}
