using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.UserDtos;

namespace ShoeBoardAPI.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<RegisterResponseDto>> ReigsterUser(RegisterUserDto registerUser);
        Task<ServiceResponse<LoginResponseDto>> LoginUser(LoginUserDto loginUser);
        string GenerateJwtToken(User user);
    }
}
