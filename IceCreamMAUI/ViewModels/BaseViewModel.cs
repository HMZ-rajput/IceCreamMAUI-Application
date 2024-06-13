using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.ViewModels
{
    public partial class BaseViewModel:ObservableObject
    {
        [ObservableProperty]
        private bool? _isBusy;

        protected async Task GoToAsync(string url, bool animate = false)=> 
            await Shell.Current.GoToAsync(url, animate: animate);

        protected async Task GoToAsync(string url, bool animate, IDictionary<string,object> parameters) =>
            await Shell.Current.GoToAsync(url, animate: animate, parameters);

        protected async Task ShowErrorMessage(string errorMessage) =>
            await Shell.Current.DisplayAlert("Error", errorMessage, "OK");

        protected async Task ShowAlertMessage(string message) =>
            await Shell.Current.DisplayAlert("Alert", message, "OK");

        protected async Task ShowToastAsync(string message)=> await Toast.Make(message).Show();
    }
}
