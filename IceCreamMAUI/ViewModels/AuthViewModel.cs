using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IceCreamMAUI.Pages;
using IceCreamMAUI.Services;
using IceCreamMAUI.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.ViewModels
{
    public partial class AuthViewModel(IAuthApi authApi, AuthService authService):BaseViewModel
    {
        private readonly IAuthApi _authApi = authApi;
        private readonly AuthService _authService = authService;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignup))]
        private string? _name;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignin)), NotifyPropertyChangedFor(nameof(CanSignup))]
        private string? _email;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignin)), NotifyPropertyChangedFor(nameof(CanSignup))]
        private string? _password;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignup))]
        private string? _address;

        public bool CanSignin => !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password);

        public bool CanSignup=>CanSignin &&
            !string.IsNullOrWhiteSpace(Name)&&
            !string.IsNullOrWhiteSpace(Address);


        [RelayCommand]
        private async Task SignupAsync()
        {
            IsBusy = true;

            try
            {
                var signupDto = new SignupRequestDto(Name, Email, Password, Address);
                //Make Api Call
                var result=await _authApi.SignupAsync(signupDto);

                if (result.IsSuccess)
                {
                    _authService.Signin(result.Data);
                    //Navigate to home
                    await GoToAsync($"//{nameof(HomePage)}", animate: true);
                }
                else
                {
                    //display error
                    await ShowErrorMessage(result.ErrorMessage ?? "Unknown error in signup");
                }

            }catch(Exception ex)
            {
                await ShowErrorMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SigninAsync()
        {
            IsBusy = true;

            try
            {
                var signinDto = new SigninRequestDto(Email, Password);
                //Make Api Call
                var result = await _authApi.SigninAsync(signinDto);

                if (result.IsSuccess)
                {
                    _authService.Signin(result.Data);
                    //Navigate to home
                    await GoToAsync($"//{nameof(HomePage)}", animate: true);
                }
                else
                {
                    //display error
                    await ShowErrorMessage(result.ErrorMessage ?? "Unknown error in signin");
                }

            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
