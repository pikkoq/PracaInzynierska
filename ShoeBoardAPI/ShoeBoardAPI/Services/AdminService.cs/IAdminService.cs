using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.AdminDtos;

namespace ShoeBoardAPI.Services.AdminService.cs
{
    public interface IAdminService
    {
        Task<SearchServiseResponse<List<GetShoesToAcceptDto>>> GetShoesToAccept(int pageNumber = 1);
        Task<ServiceResponse<bool>> AccecptNewAddedShoes(int shoeId);
        Task<ServiceResponse<bool>> DeclineNewAddedShoes(int shoeId);
        Task<ServiceResponse<bool>> EditNewAddedShoes(int shoeId, EditNewAddedShoesDto editShoeDto);
        Task<SearchServiseResponse<List<GetAllUsersDto>>> GetAllUsers(int pageNumber = 1);
        Task<ServiceResponse<bool>> DeleteUserAccount(string userId);
        Task<ServiceResponse<bool>> EditUserAccount(string userId, EditUserAccountDto editUserDto);
        Task<SearchServiseResponse<List<GetAllUsersPostsDto>>> GetAllUsersPosts(int pageNumber = 1);
        Task<ServiceResponse<bool>> EditUserPost(int postId, string content);
        Task<ServiceResponse<bool>> DeleteUserPost(int postId);

    }
}
