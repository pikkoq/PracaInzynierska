using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.UserDtos;

namespace ShoeBoardAPI.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<RegisterResponseDto>> ReigsterUser(RegisterUserDto registerUser);
        Task<ServiceResponse<LoginResponseDto>> LoginUser(LoginUserDto loginUser);
        Task<ServiceResponse<GetUserDto>> GetUser(int id);
        Task<ServiceResponse<bool>> EditUserData(EditUserDto userEdit, int id);
        Task<ServiceResponse<bool>> ChangeUserPassword(ChangePasswordDto userPassword, int id);
        string GenerateJwtToken(User user);
    }
}
