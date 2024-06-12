using IceCreamMAUI.Api.Services;
using IceCreamMAUI.Shared.Dtos;

namespace IceCreamMAUI.Api.Endpoint
{
    public static class Endpoints
    {
        public static IEndpointRouteBuilder MapEndPoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/signup", async (SignupRequestDto dto, AuthService authService) =>
           TypedResults.Ok(await authService.SignupAsync(dto)));

            app.MapPost("/api/signin",async(SigninRequestDto dto, AuthService authService)=>
            TypedResults.Ok(await authService.SigninAsync(dto)));

            app.MapGet("/api/icecreams", async (IcecreamService icecreamService) =>
            TypedResults.Ok(await icecreamService.GetIcecreamsAsync()));

            return app;
        }
    }
}
