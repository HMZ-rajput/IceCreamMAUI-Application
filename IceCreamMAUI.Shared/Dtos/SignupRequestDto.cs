using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.Shared.Dtos
{
    public record SignupRequestDto(string Name, string Email, string password, string Address);
}
