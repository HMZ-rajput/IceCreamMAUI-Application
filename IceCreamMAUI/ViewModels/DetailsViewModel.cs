using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Models;
using IceCreamMAUI.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.ViewModels
{
    //detailspage?queryId=Value
    [QueryProperty(nameof(Icecream), nameof(Icecream))]
    public partial class DetailsViewModel : BaseViewModel
    {
        public DetailsViewModel(CartViewModel cartViewModel)
        {
            _cartViewModel = cartViewModel;
        }

        private readonly CartViewModel _cartViewModel;

        [ObservableProperty]
        private IcecreamDto? _icecream;

        [ObservableProperty]
        private int _quantity;

        [ObservableProperty]
        private IcecreamOption[] _options = [];

        partial void OnIcecreamChanged(IcecreamDto? value)
        {
            Options = [];
            if (value is null)
                return;

            Options = value.Options.Select(o=>new IcecreamOption
                        {
                            Flavor = o.Flavor,
                            Topping = o.Topping,
                            IsSelected = false
                        }).ToArray();

            Quantity = _cartViewModel.GetItemCartCount(value.Id);
        }

        [RelayCommand]
        private void IncreaseQuantity() => Quantity++;
        [RelayCommand]
        private void DecreaseQuantity()
        {
            if(Quantity>0)
                Quantity--;
        }

        [RelayCommand]
        private async Task GoBackAsync() => await GoToAsync("..", animate:true);

        [RelayCommand]
        private void SelectOption(IcecreamOption newOption)
        {
            var newIsSelected = !newOption.IsSelected;
            //deselect all option
            Options = [..Options.Select(o => { o.IsSelected = false; return o; })];
            newOption.IsSelected = newIsSelected;
        }

        [RelayCommand]
        private async Task AddToCartAsync()
        {
            var selectedOption = Options.FirstOrDefault(o=> o.IsSelected) ?? Options[0];
            _cartViewModel.AddItemToCart(Icecream!, Quantity, selectedOption.Flavor, selectedOption.Topping);

            await GoBackAsync();
        }

    }
}
