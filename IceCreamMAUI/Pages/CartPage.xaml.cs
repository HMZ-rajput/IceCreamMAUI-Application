using IceCreamMAUI.ViewModels;

namespace IceCreamMAUI.Pages;

public partial class CartPage : ContentPage
{

    public CartPage(CartViewModel cartViewModel)
	{
		InitializeComponent();
        BindingContext = cartViewModel;
    }
}