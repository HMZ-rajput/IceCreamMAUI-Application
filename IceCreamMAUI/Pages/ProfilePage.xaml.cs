using IceCreamMAUI.ViewModels;

namespace IceCreamMAUI.Pages;

public partial class ProflePage : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;

    public ProflePage(ProfileViewModel profileViewModel)
	{
		InitializeComponent();
        BindingContext = profileViewModel;
        _profileViewModel = profileViewModel;
    }

    protected override void OnAppearing()
    {
        _profileViewModel.Initialize();
    }
}