using IceCreamMAUI.Shared.Dtos;
using Refit;

namespace IceCreamMAUI.Services
{
    public interface IIcecreamApi
    {
        [Get("/api/icecreams")]
        Task<IcecreamDto[]> GetIcecreamsAsync();
    }
}
