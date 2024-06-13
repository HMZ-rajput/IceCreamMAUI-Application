using IceCreamMAUI.ViewModels;

namespace IceCreamMAUI.Pages;

public partial class OrderDetailsPage : ContentPage
{

    public OrderDetailsPage(OrderDetailsViewModel orderDetailsViewModel)
	{
		InitializeComponent();
        BindingContext = orderDetailsViewModel;
    }
}