using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.Models
{
    public partial class CartItem : ObservableObject
    {
        public int Id { get; set; }
        public int IcecreamId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string FlavorName { get; set; }
        public string ToppingName { get; set; }

        [ObservableProperty, NotifyPropertyChangedFor(nameof(TotalPrice))]
        private int _quantity;

        public double TotalPrice => Price * Quantity;
    }
}
