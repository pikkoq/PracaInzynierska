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
        Task<SearchServiseResponse<List<GetShoeSearchDto>>> SearchShoes(string searchTerm, int pageNumber = 1);
        Task<SearchServiseResponse<List<GetShoeSearchDto>>> SearchUsersShoes(string searchTerm, int pageNumber = 1);
        Task<ServiceResponse<bool>> EditUserAddedShoeDetails(int shoeId, EditShoeDetailsDto updatedShoe, string userId);
        Task<ServiceResponse<bool>> EditUserShoe(int shoeId, EditUserShoeDto updatedShoe, string userId);
        Task<ServiceResponse<bool>> DeleteUserShoe(int shoeId, string userId);
        Task<ServiceResponse<List<TopPopularShoesDto>>> GetTopPoulrShoes();
        Task<ServiceResponse<GetCatalogShoeDetailsDto>> GetCatalogShoeDetails(int catalogShoeId);
    }
}
