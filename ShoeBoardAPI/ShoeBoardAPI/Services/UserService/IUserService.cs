using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.UserDtos;

namespace ShoeBoardAPI.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<bool>> ReigsterUser(RegisterUserDto registerUser);
        Task<ServiceResponse<string>> LoginUser(LoginUserDto loginUser);
        Task<ServiceResponse<GetUserDto>> GetUser(string id);
        Task<ServiceResponse<GetUserProfileDto>> GetProfile(string userName);
        Task<ServiceResponse<bool>> EditUserData(EditUserDto userEdit, string id);
        Task<ServiceResponse<bool>> ChangeUserPassword(ChangePasswordDto userPassword, string id);
        string GenerateJwtToken(User user);
    }
}
