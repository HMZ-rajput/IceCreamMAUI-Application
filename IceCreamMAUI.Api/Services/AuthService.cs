using IceCreamMAUI.Api.Data;
using IceCreamMAUI.Api.Data.Entities;
using IceCreamMAUI.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IceCreamMAUI.Api.Services
{
    public class AuthService(DataContext context, TokenService tokenService, PasswordService passwordService)
    {
        private readonly DataContext _context = context;
        private readonly TokenService _tokenService= tokenService;
        private readonly PasswordService _passwordService= passwordService;

        public async Task<ResultWithDataDto<AuthResponseDto>> SignupAsync(SignupRequestDto dto)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email == dto.Email))
            {
                return ResultWithDataDto<AuthResponseDto>.Failure("Email Already Exist");
            }
            var user = new User
            {
                Email = dto.Email,
                Address = dto.Address,
                Name = dto.Name,
            };

            (user.Salt, user.Hash) = _passwordService.GenerateSaltAndHash(dto.password);

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return GenerateAuthReponse(user);
            }
            catch (Exception ex)
            {
                return ResultWithDataDto<AuthResponseDto>.Failure(ex.Message);
            }
        }



        public async Task<ResultWithDataDto<AuthResponseDto>>  SigninAsync(SigninRequestDto dto)
        {
            var dbUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Email==dto.Email);

            if(dbUser is null)
            {
                return ResultWithDataDto<AuthResponseDto>.Failure("User Does Not Exist");
            }
            if(!_passwordService.AreEqual(dto.Password, dbUser.Salt, dbUser.Hash))
            {
                return ResultWithDataDto<AuthResponseDto>.Failure("Incorect Password");
            }
            return GenerateAuthReponse(dbUser);
        }

        private ResultWithDataDto<AuthResponseDto> GenerateAuthReponse(User user)
        {
            var loggedInUser = new LoggedInUser(user.Id, user.Name, user.Email, user.Address);
            var token = _tokenService.GenerateJwt(loggedInUser);
            var authRespone = new AuthResponseDto(loggedInUser, token);

            return ResultWithDataDto<AuthResponseDto>.Success(authRespone);
        }

    }


}
