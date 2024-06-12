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
        [ObservableProperty]
        private IcecreamDto? _icecream;

        [ObservableProperty]
        private int _quantity;

        [ObservableProperty]
        private IcecreamOption[] _options = [];

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


    }
}
