using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ServiceResponse<List<GetAllAddedUserShoesDto>>> GetAllAddedUserShoes(string userId)
        {
            var response = new ServiceResponse<List<GetAllAddedUserShoesDto>>();

            var addedShoes = await _context.UserShoeCatalogs
                .Where(x => x.UserId == userId)
                .ToListAsync();

            response.Data = _mapper.Map<List<GetAllAddedUserShoesDto>>(addedShoes);
            response.Success = true;
            response.Message = "Successfully retrived added user shoes";
            return response;
        }

        public async Task<ServiceResponse<List<GetAllUserShoesDto>>> GetAllUserShoes(string userId)
        {
            var response = new ServiceResponse<List<GetAllUserShoesDto>>();

            var userShoes = await _context.Shoes
                .Where(s => s.UserId == userId)
                .ToListAsync();

            response.Data = _mapper.Map<List<GetAllUserShoesDto>>(userShoes);
            response.Success = true;
            response.Message = "Successfully retrived user shoes";
            return response;
        }

        public async Task<ServiceResponse<GetShoeDetailsDto>> GetShoeDetails(int shoeId)
        {
            var response = new ServiceResponse<GetShoeDetailsDto>();
            var shoe = await _context.Shoes
                .Include(s => s.ShoeCatalog)
                .Include(s => s.UserShoeCatalog)
                .FirstOrDefaultAsync(s => s.Id == shoeId);

            if (shoe == null)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Shoe not found";
                return response;
            }

            response.Data = shoe.ShoeAddType == ShoeAddType.MainCatalog
                ? _mapper.Map<GetShoeDetailsDto>(shoe.ShoeCatalog)
                : _mapper.Map<GetShoeDetailsDto>(shoe.UserShoeCatalog);
            response.Success = true;
            response.Message = "Shoe details retrived successfully.";
            return response;
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

        public async Task<SearchServiseResponse<List<GetShoeSearchDto>>> SearchShoes(string searchTerm, int pageNumber = 1)
        {
            var response = new SearchServiseResponse<List<GetShoeSearchDto>>();
            int pageSize = 40;
            int offset = (pageNumber - 1) * pageSize;

            string BuildSearchQuery(string baseQuery, string searchTerm)
            {
                var searchWords = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var searchConditions = string.Join(" AND ", searchWords.Select(word => $"CONTAINS((Title, Brand, Model_No, Nickname, Series, Main_Color), '\"{word}\"')"));

                return $"{baseQuery} WHERE {searchConditions}";
            }

            var shoeCatalogQuery = @"
                SELECT *
                FROM ShoeCatalogs
                ";

            var shoeCatalogQueryCount = @"
                SELECT Title
                FROM ShoeCatalogs
                ";

            var dynamicShoeCatalogQuery = BuildSearchQuery(shoeCatalogQuery, searchTerm) + $" ORDER BY Id OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY;";

            var shoeCatalog = await _context.ShoeCatalogs
                .FromSqlRaw(dynamicShoeCatalogQuery)
                .ToListAsync();

            var totalCountShoeCatalog = await _context.ShoeCatalogs
                .FromSqlRaw(BuildSearchQuery(shoeCatalogQueryCount, searchTerm))
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalCountShoeCatalog / pageSize);

            var catalogShoeDto = _mapper.Map<List<GetShoeSearchDto>>(shoeCatalog);

            response.Data = catalogShoeDto;
            response.TotalCount = totalCountShoeCatalog;
            response.TotalPages = totalPages;
            response.Message = "Search results retrieved successfully.";
            response.Success = true;
            return response;
        }

        public async Task<SearchServiseResponse<List<GetShoeSearchDto>>> SearchUsersShoes(string searchTerm, int pageNumber = 1)
        {
            var response = new SearchServiseResponse<List<GetShoeSearchDto>>();
            int pageSize = 40;
            int offset = (pageNumber - 1) * pageSize;

            string BuildSearchQuery(string baseQuery, string searchTerm)
            {
                var searchWords = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var searchConditions = string.Join(" AND ", searchWords.Select(word => $"CONTAINS((Title, Brand, Model_No, Nickname, Series, Main_Color), '\"{word}\"')"));

                return $"{baseQuery} WHERE {searchConditions}";
            }

            var shoeCatalogQuery = @"
                SELECT *
                FROM UserShoeCatalogs
                ";

            var shoeCatalogQueryCount = @"
                SELECT Title
                FROM UserShoeCatalogs
                ";

            var dynamicShoeCatalogQuery = BuildSearchQuery(shoeCatalogQuery, searchTerm) + $" ORDER BY Id OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY;";

            var shoeCatalog = await _context.UserShoeCatalogs
                .FromSqlRaw(dynamicShoeCatalogQuery)
                .ToListAsync();

            var totalCountShoeCatalog = await _context.UserShoeCatalogs
                .FromSqlRaw(BuildSearchQuery(shoeCatalogQueryCount, searchTerm))
                .CountAsync();

            int totalPages = (int)Math.Ceiling((double)totalCountShoeCatalog / pageSize);

            var catalogShoeDto = _mapper.Map<List<GetShoeSearchDto>>(shoeCatalog);

            response.Data = catalogShoeDto;
            response.TotalCount = totalCountShoeCatalog;
            response.TotalPages = totalPages;
            response.Message = "Search results retrieved successfully.";
            response.Success = true;
            return response;
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
