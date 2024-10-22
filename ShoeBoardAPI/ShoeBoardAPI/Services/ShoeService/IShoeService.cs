using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.ShoeDtos;

namespace ShoeBoardAPI.Services.ShoeService
{
    public interface IShoeService
    {
        Task<ServiceResponse<bool>> NewShoeRegistry(NewShoeRegistryDto newShoe, string userId);
        Task<ServiceResponse<bool>> SignShoeToUser(SignShoeToUserDto newShoe, int? shoeCatalogId, int? userShoeCatalogId, string userId);
        Task<ServiceResponse<List<GetAllUserShoesDto>>> GetAllUserShoes(string userId);
        Task<ServiceResponse<List<GetAllAddedUserShoesDto>>> GetAllAddedUserShoes(string userId);
        Task<ServiceResponse<GetShoeDetailsDto>> GetShoeDetails(int shoeId);
        Task<ServiceResponse<List<GetShoeDetailsDto>>> SearchShoes(string searchTerm);
        Task<ServiceResponse<bool>> EditShoeDetails();
        Task<ServiceResponse<bool>> EditAddedUserShoe();
        Task<ServiceResponse<bool>> DeleteUserShoe();
    }
}
