using AutoMapper;
using Microsoft.Extensions.Configuration.UserSecrets;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using ShoeBoardAPI.Models.Enums;

namespace ShoeBoardAPI.Services.ShoeService
{
    public class ShoeService : IShoeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ShoeService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Task<ServiceResponse<bool>> DeleteUserShoe()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> EditAddedUserShoe()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> EditShoeDetails()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<GetAllAddedUserShoesDto>>> GetAllAddedUserShoes(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<GetAllUserShoesDto>>> GetAllUserShoes(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<GetShoeDetailsDto>> GetShoeDetails(string shoeId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<bool>> NewShoeRegistry(NewShoeRegistryDto newShoe, string userId)
        {
            var response = new ServiceResponse<bool>();
            var shoeCatalog = _mapper.Map<UserShoeCatalog>(newShoe);

            if(string.IsNullOrEmpty(shoeCatalog.Title) || string.IsNullOrEmpty(shoeCatalog.Main_Color) ||
                string.IsNullOrEmpty(shoeCatalog.Brand) || string.IsNullOrEmpty(shoeCatalog.Nickname) || 
                string.IsNullOrEmpty(shoeCatalog.Gender) || string.IsNullOrEmpty(shoeCatalog.Series))
            {
                response.Success = false;
                response.Message = "Title, Brand, Nickname, Gender, Main_color and series can't be empty.";
                return response;
            }

            shoeCatalog.UserId = userId;

            await _context.UserShoeCatalogs.AddAsync(shoeCatalog);
            var result = await _context.SaveChangesAsync();

            response.Success = result > 0;
            if (response.Success)
            {
                response.Message = "New shoe added to catalog.";
                response.Data = true;
                return response;
            }
            response.Message = "Failed to add new shoe";
            response.Data = false;
            return response;
        }

        public Task<ServiceResponse<List<GetShoeDetailsDto>>> SearchShoes(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<bool>> SignShoeToUser(SignShoeToUserDto newShoe, int? shoeCatalogId, int? userShoeCatalogId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var shoe = new Shoe
            {
                UserId = userId,
                Size = newShoe.Size,
                ComfortRating = newShoe.ComfortRating,
                StyleRating = newShoe.StyleRating,
                Season = newShoe.Season,
                Review = newShoe.Review,
                DateAdded = DateTime.UtcNow,
            };

            if(shoeCatalogId != null)
            {
                shoe.ShoeCatalogId = shoeCatalogId;
                shoe.ShoeAddType = ShoeAddType.MainCatalog;
            }
            else if(userShoeCatalogId != null)
            {
                shoe.UserShoeCatalogId = userShoeCatalogId;
                shoe.ShoeAddType = ShoeAddType.UserCatalog;
            }
            else
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Failed to find shoe.";
            }

            await _context.AddAsync(shoe);
            var result = await _context.SaveChangesAsync();

            response.Success = result > 0;
            if (response.Success)
            {
                response.Message = "Shoe successfully added to user's collection.";
                response.Data = true;
                return response;
            }
            response.Message = "Error adding shoe to user's collection.";
            response.Data = false;
            return response;

        }
    }
}
