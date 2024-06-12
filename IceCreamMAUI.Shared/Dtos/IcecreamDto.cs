using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.Shared.Dtos
{
    public record IcecreamOptionDto(string Flavor, string Topping);
    public record IcecreamDto(int Id, string Name, string Image, double Price, IcecreamOptionDto[] Options);

}
