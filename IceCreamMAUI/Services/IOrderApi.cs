using IceCreamMAUI.Shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamMAUI.Services
{
    [Headers("Authorization: Bearer")]
    public interface IOrderApi
    {
        [Post("/api/order/plave-order")]
        Task<ResultDto> PlaceOrderAsync(OrderPlaceDto dto);
    }
}
